namespace Edelstein.Plugin.Rue.ClientAnalysis.NativeTypes;

public sealed class NativeAllocator(ProcessHandle process, MemoryAccessor memory) : IDisposable
{
    private readonly ProcessHandle _process = process;
    private readonly MemoryAccessor _memory = memory;
    private readonly HashSet<IntPtr> _allocations = [];

    public IntPtr Allocate(int size)
    {
        if (!_process.IsAttached || size <= 0)
            return IntPtr.Zero;

        var allocated = Win32Api.VirtualAllocEx(
            _process.Handle,
            IntPtr.Zero,
            (uint)size,
            Win32Api.AllocationType.Commit | Win32Api.AllocationType.Reserve,
            Win32Api.MemoryProtection.ReadWrite);

        if (allocated != IntPtr.Zero)
            _allocations.Add(allocated);

        return allocated;
    }

    public void Free(IntPtr address)
    {
        if (address == IntPtr.Zero || !_process.IsAttached)
            return;

        Win32Api.VirtualFreeEx(_process.Handle, address, 0, Win32Api.FreeType.Release);
        _allocations.Remove(address);
    }

    public bool WriteBytes(IntPtr address, byte[] buffer) => _memory.WriteBytes(address, buffer);

    public void Dispose()
    {
        if (!_process.IsAttached)
        {
            _allocations.Clear();
            return;
        }

        foreach (var ptr in _allocations)
            Win32Api.VirtualFreeEx(_process.Handle, ptr, 0, Win32Api.FreeType.Release);

        _allocations.Clear();
    }
}
