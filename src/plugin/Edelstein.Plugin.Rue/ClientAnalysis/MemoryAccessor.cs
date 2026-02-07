using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class MemoryAccessor(ProcessHandle process, ILogger? logger)
{
    private readonly ProcessHandle _process = process;
    private readonly ILogger? _logger = logger;

    private const int DefaultStableReadRetries = 3;
    private const int DefaultStableReadDelayMs = 1;

    public bool IsReadable(IntPtr address, int size)
    {
        if (!_process.IsAttached)
            return false;

        if (address == IntPtr.Zero)
            return false;

        var query = Win32Api.VirtualQueryEx(
            _process.Handle,
            address,
            out var mbi,
            (uint)Marshal.SizeOf<Win32Api.MEMORY_BASIC_INFORMATION>());

        if (query == 0)
            return false;

        if (mbi.State != Win32Api.MemoryState.Commit)
            return false;

        var protect = mbi.Protect;
        if ((protect & Win32Api.MemoryProtection.NoAccess) != 0 || (protect & Win32Api.MemoryProtection.Guard) != 0)
            return false;

        var regionSize = (long)mbi.RegionSize;
        var baseAddr = (long)mbi.BaseAddress;
        var start = (long)address;
        if (start < baseAddr)
            return false;

        var offset = start - baseAddr;
        return offset + size <= regionSize;
    }

    public bool TryReadInt32(IntPtr address, out int value) => TryReadInt32(address, out value, out _);

    public bool TryReadInt32(IntPtr address, out int value, out int error)
    {
        value = 0;
        error = 0;

        if (!_process.IsAttached)
            return false;

        var buffer = new byte[TypeSizes.Int32];
        if (!Win32Api.ReadProcessMemory(_process.Handle, address, buffer, TypeSizes.Int32, out var bytesRead) || bytesRead != TypeSizes.Int32)
        {
            error = Marshal.GetLastWin32Error();
            return false;
        }

        value = BinaryPrimitives.ReadInt32LittleEndian(buffer);
        return true;
    }

    public int? ReadInt32Stable(IntPtr address, int retries = DefaultStableReadRetries, int delayMs = DefaultStableReadDelayMs)
    {
        if (!_process.IsAttached)
            return null;

        var buffer1 = new byte[TypeSizes.Int32];
        var buffer2 = new byte[TypeSizes.Int32];

        for (var i = 0; i < retries; i++)
        {
            if (!Win32Api.ReadProcessMemory(_process.Handle, address, buffer1, TypeSizes.Int32, out var bytesRead1) || bytesRead1 != TypeSizes.Int32)
                return null;

            if (delayMs > 0)
                Thread.Sleep(delayMs);

            if (!Win32Api.ReadProcessMemory(_process.Handle, address, buffer2, TypeSizes.Int32, out var bytesRead2) || bytesRead2 != TypeSizes.Int32)
                return null;

            if (buffer1[0] == buffer2[0] && buffer1[1] == buffer2[1] && buffer1[2] == buffer2[2] && buffer1[3] == buffer2[3])
                return BinaryPrimitives.ReadInt32LittleEndian(buffer1);
        }

        return BinaryPrimitives.ReadInt32LittleEndian(buffer2);
    }

    public uint? ReadUInt32Stable(IntPtr address, int retries = DefaultStableReadRetries, int delayMs = DefaultStableReadDelayMs)
    {
        var value = ReadInt32Stable(address, retries, delayMs);
        return value.HasValue ? unchecked((uint)value.Value) : null;
    }

    public byte? ReadByte(IntPtr address)
    {
        if (!_process.IsAttached)
            return null;

        var buffer = new byte[1];
        if (Win32Api.ReadProcessMemory(_process.Handle, address, buffer, 1, out var bytesRead) && bytesRead == 1)
            return buffer[0];

        return null;
    }

    public string? ReadCString(IntPtr address, int maxBytes = 64)
    {
        if (!_process.IsAttached)
            return null;

        if (address == IntPtr.Zero)
            return null;

        if (maxBytes <= 0)
            return string.Empty;

        var bytes = new byte[maxBytes];
        var count = 0;

        for (var i = 0; i < maxBytes; i++)
        {
            var b = ReadByte(IntPtr.Add(address, i));
            if (!b.HasValue)
                break;

            if (b.Value == 0)
                break;

            bytes[count++] = b.Value;
        }

        return count == 0 ? string.Empty : Encoding.ASCII.GetString(bytes, 0, count);
    }

    public bool WriteInt32WithVerification(IntPtr address, int value)
    {
        if (!_process.IsAttached)
            return false;

        var bytes = new byte[TypeSizes.Int32];
        BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
        if (!Win32Api.WriteProcessMemory(_process.Handle, address, bytes, TypeSizes.Int32, out var bytesWritten) || bytesWritten != TypeSizes.Int32)
        {
            var error = Marshal.GetLastWin32Error();
            _logger?.LogError("[Rue-CMemory] Failed to write Int32 at 0x{Address:X8}. Error: {Error}", address.ToInt32(), error);
            return false;
        }

        var verify = ReadInt32Stable(address, retries: 1, delayMs: 0);
        if (verify != value)
        {
            _logger?.LogWarning("[Rue-CMemory] Verification mismatch at 0x{Address:X8}: wrote {Expected}, read {Actual}",
                address.ToInt32(), value, verify);
            return false;
        }

        return true;
    }

    public bool WriteBytes(IntPtr address, byte[] buffer)
    {
        if (!_process.IsAttached)
            return false;

        if (!Win32Api.WriteProcessMemory(_process.Handle, address, buffer, buffer.Length, out var written))
            return false;

        return written == buffer.Length;
    }

    public byte[]? ReadMemory(IntPtr address, int size)
    {
        if (!_process.IsAttached)
            return null;

        var buffer = new byte[size];
        if (Win32Api.ReadProcessMemory(_process.Handle, address, buffer, size, out var bytesRead) && bytesRead == size)
            return buffer;

        return null;
    }
}
