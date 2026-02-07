using System.Runtime.InteropServices;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

internal static class Win32Api
{
    [Flags]
    internal enum ProcessAccess : uint
    {
        VmOperation = 0x0008,
        VmRead = 0x0010,
        VmWrite = 0x0020,
        QueryInformation = 0x0400
    }

    [Flags]
    internal enum AllocationType : uint
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Release = 0x8000
    }

    [Flags]
    internal enum MemoryProtection : uint
    {
        NoAccess = 0x01,
        ReadWrite = 0x04,
        ExecuteReadWrite = 0x40,
        Guard = 0x100
    }

    internal enum MemoryState : uint
    {
        Commit = 0x1000
    }

    [Flags]
    internal enum FreeType : uint
    {
        Release = 0x8000
    }

    internal enum WaitResult : uint
    {
        Object0 = 0x00000000,
        Timeout = 0x00000102,
        Failed = 0xFFFFFFFF
    }

    internal enum NtStatus : uint
    {
        AccessViolation = 0xC0000005,
        InPageError = 0xC0000006,
        IllegalInstruction = 0xC000001D,
        NonContinuableException = 0xC0000025,
        StackOverflow = 0xC00000FD,
        IntegerDivideByZero = 0xC0000094,
        IntegerOverflow = 0xC0000095,
        ArrayBoundsExceeded = 0xC000008C,
        FloatDenormalOperand = 0xC000008D,
        FloatDivideByZero = 0xC000008E,
        FloatInexactResult = 0xC000008F,
        FloatInvalidOperation = 0xC0000090,
        FloatOverflow = 0xC0000091,
        FloatStackCheck = 0xC0000092,
        FloatUnderflow = 0xC0000093,
        PrivilegedInstruction = 0xC0000096,
        GuardPageViolation = 0x80000001,
        DatatypeMisalignment = 0x80000002,
        Breakpoint = 0x80000003,
        SingleStep = 0x80000004,
        CppException = 0xE06D7363,
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int dwSize,
        out int lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int nSize,
        out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool VirtualProtectEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        int dwSize,
        MemoryProtection flNewProtect,
        out MemoryProtection lpflOldProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr VirtualAllocEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        AllocationType flAllocationType,
        MemoryProtection flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool VirtualFreeEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        FreeType dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        out uint lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern WaitResult WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

    [DllImport("kernel32.dll")]
    internal static extern int VirtualQueryEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        out MEMORY_BASIC_INFORMATION lpBuffer,
        uint dwLength);

    [StructLayout(LayoutKind.Sequential)]
    internal struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public MemoryProtection AllocationProtect;
        public IntPtr RegionSize;
        public MemoryState State;
        public MemoryProtection Protect;
        public uint Type;
    }
}
