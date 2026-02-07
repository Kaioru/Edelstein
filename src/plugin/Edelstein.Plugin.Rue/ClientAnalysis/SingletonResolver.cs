using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class SingletonResolver(ProcessHandle process, MemoryAccessor memory, ILogger? logger)
{
    private readonly ProcessHandle _process = process;
    private readonly MemoryAccessor _memory = memory;
    private readonly ILogger? _logger = logger;

    private IntPtr _cwvsContextBase = IntPtr.Zero;
    private IntPtr _cloginBase = IntPtr.Zero;
    private IntPtr _cuiChannelSelectBase = IntPtr.Zero;
    private IntPtr _cuiWorldSelectBase = IntPtr.Zero;

    public IntPtr CWvsContextBase => _cwvsContextBase;
    public IntPtr CLoginBase => _cloginBase;
    public IntPtr CUIChannelSelectBase => _cuiChannelSelectBase;
    public IntPtr CUIWorldSelectBase => _cuiWorldSelectBase;

    public bool FindCWvsContext()
    {
        if (!_process.IsAttached || _process.HasExited)
            return false;

        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CWvsContextSingletonPtr));
        if (!_memory.TryReadInt32(ptrAddress, out var value, out var error))
        {
            if (_process.TryMarkExitedFromError(error))
                return false;
            _logger?.LogDebug("[Rue-CMemory] Failed to read CWvsContext pointer. Error: {Error}", error);
            return false;
        }

        var newBase = new IntPtr(value);
        if (newBase == IntPtr.Zero)
        {
            _logger?.LogDebug("[Rue-CMemory] CWvsContext pointer is NULL - context not yet created");
            return false;
        }

        if (_cwvsContextBase != newBase)
        {
            _cwvsContextBase = newBase;
            _logger?.LogDebug("[Rue-CMemory] Found CWvsContext at 0x{Address:X8}", _cwvsContextBase.ToInt32());
        }

        return true;
    }

    public bool FindCLogin()
    {
        if (!_process.IsAttached || _process.HasExited)
            return false;

        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CUIChannelSelectSingletonPtr));
        return TryResolveCLoginViaPointer(ptrAddress, V95ClientStructs.Offsets.CUIChannelSelect.Login, "CUIChannelSelect");
    }

    public bool FindCUIChannelSelect()
    {
        if (!_process.IsAttached)
            return false;

        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CUIChannelSelectSingletonPtr));
        if (!_memory.TryReadInt32(ptrAddress, out var value))
            return false;

        _cuiChannelSelectBase = new IntPtr(value);
        return _cuiChannelSelectBase != IntPtr.Zero;
    }

    public bool FindCUIWorldSelect()
    {
        if (!_process.IsAttached)
            return false;

        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CUIWorldSelectSingletonPtr));
        if (!_memory.TryReadInt32(ptrAddress, out var value))
            return false;

        _cuiWorldSelectBase = new IntPtr(value);
        return _cuiWorldSelectBase != IntPtr.Zero;
    }

    public bool? IsCUIChannelSelectValid()
    {
        if (!_process.IsAttached)
            return null;

        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CUIChannelSelectSingletonPtr));
        if (!_memory.TryReadInt32(ptrAddress, out var value))
            return null;

        return value != 0;
    }

    public bool? IsCUIWorldSelectValid()
    {
        if (!_process.IsAttached)
            return null;

        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CUIWorldSelectSingletonPtr));
        if (!_memory.TryReadInt32(ptrAddress, out var value))
            return null;

        return value != 0;
    }

    public bool? IsCLoginGradeWndValid()
    {
        if (!_process.IsAttached)
            return null;

        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CLoginGradeWndSingletonPtr));
        if (!_memory.TryReadInt32(ptrAddress, out var value))
            return null;

        return value != 0;
    }

    public void RefreshPointers()
    {
        if (_process.HasExited)
            return;

        FindCWvsContext();
        FindCLogin();
    }

    private bool TryResolveCLoginViaPointer(IntPtr singletonPtrAddress, int loginOffset, string sourceName)
    {
        if (!_memory.TryReadInt32(singletonPtrAddress, out var singletonValue))
            return false;

        var singletonBase = new IntPtr(singletonValue);
        if (singletonBase == IntPtr.Zero)
            return false;

        var loginPtrAddr = IntPtr.Add(singletonBase, loginOffset);
        if (!_memory.TryReadInt32(loginPtrAddr, out var loginValue))
            return false;

        var newLoginBase = new IntPtr(loginValue);
        if (newLoginBase == IntPtr.Zero)
            return false;

        var addr = newLoginBase.ToInt32();
        if (addr < 0x10000 || addr > 0x7FFFFFFF)
        {
            _logger?.LogDebug("[Rue-CMemory] Rejected CLogin candidate from {Source}: 0x{Addr:X8} (out of range)",
                sourceName, addr);
            return false;
        }

        if (!ValidateCLoginPointer(newLoginBase, sourceName))
            return false;

        if (_cloginBase != newLoginBase)
        {
            _cloginBase = newLoginBase;
            _logger?.LogDebug("[Rue-CMemory] Found CLogin via {Source}->m_pLogin at 0x{Address:X8}",
                sourceName, _cloginBase.ToInt32());
        }

        return true;
    }

    private bool ValidateCLoginPointer(IntPtr candidateBase, string sourceName)
    {
        var stepAddr = IntPtr.Add(candidateBase, V95ClientStructs.Offsets.CLogin.LoginStep);
        var stepVal = _memory.ReadInt32Stable(stepAddr, retries: 1, delayMs: 0);
        if (stepVal.HasValue && (stepVal.Value < 0 || stepVal.Value > 10))
        {
            _logger?.LogDebug(
                "[Rue-CMemory] Rejected CLogin candidate from {Source}: 0x{Addr:X8} - LoginStep={Step} (garbage)",
                sourceName, candidateBase.ToInt32(), stepVal.Value);
            return false;
        }

        var charSelAddr = IntPtr.Add(candidateBase, V95ClientStructs.Offsets.CLogin.CharSelected);
        var charSelVal = _memory.ReadInt32Stable(charSelAddr, retries: 1, delayMs: 0);
        if (charSelVal.HasValue && (charSelVal.Value < -1 || charSelVal.Value > 50))
        {
            _logger?.LogDebug(
                "[Rue-CMemory] Rejected CLogin candidate from {Source}: 0x{Addr:X8} - CharSelected={Val} (garbage)",
                sourceName, candidateBase.ToInt32(), charSelVal.Value);
            return false;
        }

        return true;
    }
}
