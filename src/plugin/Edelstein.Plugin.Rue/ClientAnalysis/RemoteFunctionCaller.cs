using System.Runtime.InteropServices;
using System.Text;
using Iced.Intel;
using Microsoft.Extensions.Logging;
using IcedDecoder = Iced.Intel.Decoder;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

/// <summary>
/// Executes functions in the v95 client process remotely.
///
/// ## How It Works:
/// 1. Allocate memory in target process for shellcode
/// 2. Write x86 assembly that calls the target function
/// 3. Create a remote thread to execute the shellcode
/// 4. Wait for completion and read return value
///
/// ## CLogin::SendLoginPacket Specifics:
/// - Calling convention: __thiscall (this pointer in ECX)
/// - Parameters: int nWorldID, int nChannelID (pushed on stack right-to-left)
/// - Returns: int (0 if already sent, or return from sub_C9E6D4)
///
/// ## Shellcode Template:
/// ```asm
/// ; __thiscall CLogin::SendLoginPacket(int nWorldID, int nChannelID)
/// push channelID      ; 68 XX XX XX XX
/// push worldID        ; 68 XX XX XX XX
/// mov ecx, pCLogin    ; B9 XX XX XX XX  (this pointer)
/// call SendLoginPacket; E8 XX XX XX XX  (relative call)
/// ret                 ; C3
/// ```
/// </summary>
public class RemoteFunctionCaller(ILogger? logger, IntPtr processHandle) : IDisposable
{
    private readonly ILogger? _logger = logger;
    private readonly IntPtr _processHandle = processHandle;
    private readonly ShellcodeFactory _shellcodeFactory = new();
    private IntPtr _shellcodeAddr = IntPtr.Zero;
    private const int SHELLCODE_SIZE = 256;

    private const Win32Api.AllocationType AllocationType = Win32Api.AllocationType.Commit | Win32Api.AllocationType.Reserve;
    private const Win32Api.FreeType FreeType = Win32Api.FreeType.Release;
    private const Win32Api.MemoryProtection Protection = Win32Api.MemoryProtection.ExecuteReadWrite;

    /// <summary>
    /// Calls CLogin::SendLoginPacket(nWorldID, nChannelID) in the target process.
    /// </summary>
    /// <param name="cLoginInstance">Pointer to the CLogin instance (this pointer)</param>
    /// <param name="sendLoginPacketAddr">Address of CLogin::SendLoginPacket function</param>
    /// <param name="worldId">World ID to select</param>
    /// <param name="channelId">Channel ID to select</param>
    /// <returns>True if the remote call succeeded, false otherwise</returns>
    public bool CallSendLoginPacket(IntPtr cLoginInstance, IntPtr sendLoginPacketAddr, int worldId, int channelId)
    {
        if (_processHandle == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-RemoteCall] Process handle is invalid");
            return false;
        }

        if (cLoginInstance == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-RemoteCall] CLogin instance pointer is NULL");
            return false;
        }

        if (sendLoginPacketAddr == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-RemoteCall] SendLoginPacket address is NULL");
            return false;
        }

        _logger?.LogInformation(
            "[Rue-RemoteCall] Calling CLogin::SendLoginPacket(worldId={WorldId}, channelId={ChannelId}) " +
            "at 0x{FuncAddr:X8} with this=0x{This:X8}",
            worldId, channelId, sendLoginPacketAddr.ToInt32(), cLoginInstance.ToInt32());

        // Allocate memory for shellcode
        if (_shellcodeAddr == IntPtr.Zero)
        {
            _shellcodeAddr = Win32Api.VirtualAllocEx(_processHandle, IntPtr.Zero, SHELLCODE_SIZE, AllocationType, Protection);
            if (_shellcodeAddr == IntPtr.Zero)
            {
                _logger?.LogError("[Rue-RemoteCall] Failed to allocate shellcode memory: 0x{Error:X8}", Marshal.GetLastWin32Error());
                return false;
            }
            _logger?.LogDebug("[Rue-RemoteCall] Allocated shellcode at 0x{Addr:X8}", _shellcodeAddr.ToInt32());
        }

        // Build shellcode for __thiscall CLogin::SendLoginPacket(int, int)
        var shellcode = _shellcodeFactory.BuildSendLoginPacketShellcode(
            _shellcodeAddr,
            cLoginInstance,
            sendLoginPacketAddr,
            worldId,
            channelId);

        _logger?.LogDebug("[Rue-RemoteCall] Shellcode ({Len} bytes): {Hex}", shellcode.Length, BitConverter.ToString(shellcode));

        // Write shellcode to target process
        if (!Win32Api.WriteProcessMemory(_processHandle, _shellcodeAddr, shellcode, shellcode.Length, out var bytesWritten) || bytesWritten != shellcode.Length)
        {
            _logger?.LogError("[Rue-RemoteCall] Failed to write shellcode: 0x{Error:X8}", Marshal.GetLastWin32Error());
            return false;
        }

        // Create remote thread to execute shellcode
        var threadHandle = Win32Api.CreateRemoteThread(_processHandle, IntPtr.Zero, 0, _shellcodeAddr, IntPtr.Zero, 0, out var threadId);
        if (threadHandle == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-RemoteCall] Failed to create remote thread: 0x{Error:X8}", Marshal.GetLastWin32Error());
            return false;
        }

        _logger?.LogDebug("[Rue-RemoteCall] Created remote thread {ThreadId}, waiting for completion...", threadId);

        // Wait for thread completion (max 10 seconds)
        var waitResult = Win32Api.WaitForSingleObject(threadHandle, 10000);
        if (waitResult != Win32Api.WaitResult.Object0)
        {
            _logger?.LogError("[Rue-RemoteCall] Wait failed or timed out: {Result}", waitResult);
            Win32Api.CloseHandle(threadHandle);
            return false;
        }

        // Get return value
        if (Win32Api.GetExitCodeThread(threadHandle, out var exitCode))
        {
            _logger?.LogDebug("[Rue-RemoteCall] SendLoginPacket returned: {ReturnValue}", exitCode);
        }

        Win32Api.CloseHandle(threadHandle);
        return true;
    }

    /// <summary>
    /// Reads deobfuscated code from a function address at runtime.
    /// Themida unpacks VM-protected code when executed, so we can read it after the process starts.
    /// </summary>
    /// <param name="functionAddr">Address of the function to read</param>
    /// <param name="size">Number of bytes to read</param>
    /// <returns>Raw bytes of the function, or null on failure</returns>
    public byte[]? ReadFunctionBytes(IntPtr functionAddr, int size = 512)
    {
        if (_processHandle == IntPtr.Zero || functionAddr == IntPtr.Zero)
            return null;

        var buffer = new byte[size];
        if (Win32Api.ReadProcessMemory(_processHandle, functionAddr, buffer, size, out var bytesRead) && bytesRead > 0)
        {
            _logger?.LogDebug("[Rue-RemoteCall] Read {Bytes} bytes from function at 0x{Addr:X8}", bytesRead, functionAddr.ToInt32());
            return buffer[..bytesRead];
        }

        _logger?.LogWarning("[Rue-RemoteCall] Failed to read function at 0x{Addr:X8}: 0x{Error:X8}", functionAddr.ToInt32(), Marshal.GetLastWin32Error());
        return null;
    }

    /// <summary>
    /// Disassembles function bytes using Iced x86 decoder. Stops at the first RET instruction.
    /// </summary>
    public static string DisassembleBasic(byte[] bytes, IntPtr baseAddr, int maxInstructions = 100)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Disassembly at 0x{baseAddr.ToInt32():X8}:");
        sb.AppendLine();

        var reader = new ByteArrayCodeReader(bytes);
        var decoder = IcedDecoder.Create(32, reader);
        decoder.IP = (ulong)baseAddr.ToInt32();

        var formatter = new IntelFormatter();
        formatter.Options.SpaceAfterOperandSeparator = true;
        formatter.Options.HexPrefix = "0x";
        formatter.Options.HexSuffix = null;
        formatter.Options.UppercaseHex = false;
        var output = new StringOutput();

        var count = 0;
        var endAddress = (ulong)(baseAddr.ToInt32() + bytes.Length);

        while (decoder.IP < endAddress && count < maxInstructions)
        {
            var ip = decoder.IP;
            var instr = decoder.Decode();
            count++;

            if (instr.IsInvalid)
            {
                var badByte = bytes[(int)(ip - (ulong)baseAddr.ToInt32())];
                sb.AppendLine($"  {ip:x8}: {badByte:x2}                       db 0x{badByte:x2}");
                continue;
            }

            formatter.Format(instr, output);
            var text = output.ToStringAndReset();

            var offset = (int)(ip - (ulong)baseAddr.ToInt32());
            var hexParts = new StringBuilder();
            for (var b = 0; b < instr.Length && offset + b < bytes.Length; b++)
                hexParts.Append($"{bytes[offset + b]:x2} ");

            sb.AppendLine($"  {ip:x8}: {hexParts.ToString().TrimEnd(),-24} {text}");

            if (instr.FlowControl == FlowControl.Return)
                break;
        }

        return sb.ToString();
    }

    public void Dispose()
    {
        if (_shellcodeAddr != IntPtr.Zero && _processHandle != IntPtr.Zero)
        {
            Win32Api.VirtualFreeEx(_processHandle, _shellcodeAddr, 0, FreeType);
            _shellcodeAddr = IntPtr.Zero;
        }

        GC.SuppressFinalize(this);
    }
}
