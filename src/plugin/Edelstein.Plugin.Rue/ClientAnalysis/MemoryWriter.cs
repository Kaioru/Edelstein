using Edelstein.Plugin.Rue.ClientAnalysis.NativeTypes;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class MemoryWriter : IDisposable
{
    private readonly ILogger? _logger;
    private readonly string _processName;
    private readonly ProcessHandle _process;
    private readonly MemoryAccessor _memory;
    private readonly NativeAllocator _allocator;
    private readonly SingletonResolver _resolver;
    private readonly FieldAccessor _fields;

    public MemoryWriter(ILogger? logger, string? processName = null)
    {
        _logger = logger;
        _processName = string.IsNullOrWhiteSpace(processName) ? "localhost" : processName;
        _process = new ProcessHandle(logger);
        _memory = new MemoryAccessor(_process, logger);
        _allocator = new NativeAllocator(_process, _memory);
        _resolver = new SingletonResolver(_process, _memory, logger);
        _fields = new FieldAccessor(_memory, _resolver, _allocator, logger);
    }

    public bool TryAttach() => _process.TryAttach(_processName);

    public void Detach()
    {
        _allocator.Dispose();
        _process.Detach();
    }

    public bool HasProcessExited => _process.HasExited;

    public IntPtr ProcessHandle => _process.Handle;

    public IntPtr CLoginBase => _resolver.CLoginBase;

    public bool FindCWvsContext() => _resolver.FindCWvsContext();

    public bool FindCLogin() => _resolver.FindCLogin();

    public bool FindCUIChannelSelect() => _resolver.FindCUIChannelSelect();

    public bool FindCUIWorldSelect() => _resolver.FindCUIWorldSelect();

    public void RefreshPointers() => _resolver.RefreshPointers();

    public bool? IsCUIChannelSelectValid() => _resolver.IsCUIChannelSelectValid();

    public bool? IsCUIWorldSelectValid() => _resolver.IsCUIWorldSelectValid();

    public bool? IsCLoginGradeWndValid() => _resolver.IsCLoginGradeWndValid();

    public bool? IsConnectionDlgValid() => _fields.IsConnectionDlgValid();

    public bool SetWorldAndChannel(int worldId, int channelId, IntPtr? cwvsContextBase = null)
        => _fields.SetWorldAndChannel(worldId, channelId, cwvsContextBase);

    public bool SetQuestManWorldId(int worldId) => _fields.SetQuestManWorldId(worldId);

    public int? ReadWorldId(IntPtr? cwvsContextBase = null) => _fields.ReadWorldId(cwvsContextBase);

    public int? ReadChannelId(IntPtr? cwvsContextBase = null) => _fields.ReadChannelId(cwvsContextBase);

    public uint? ReadAccountId(IntPtr? cwvsContextBase = null) => _fields.ReadAccountId(cwvsContextBase);

    public int? ReadLoginStep(IntPtr? cloginBase = null) => _fields.ReadLoginStep(cloginBase);

    public int? ReadStepChanging(IntPtr? cloginBase = null) => _fields.ReadStepChanging(cloginBase);

    public int? ReadCharacterCount(IntPtr? cwvsContextBase = null) => _fields.ReadCharacterCount(cwvsContextBase);

    public int? ReadSlotCount(IntPtr? cwvsContextBase = null) => _fields.ReadSlotCount(cwvsContextBase);

    public int? ReadChannelNameArrayPtr(IntPtr? cwvsContextBase = null) => _fields.ReadChannelNameArrayPtr(cwvsContextBase);

    public int? ReadAdultChannelArrayPtr(IntPtr? cwvsContextBase = null) => _fields.ReadAdultChannelArrayPtr(cwvsContextBase);

    public int? ReadCharSelected(IntPtr? cloginBase = null) => _fields.ReadCharSelected(cloginBase);

    public byte? ReadLoginOpt(IntPtr? cloginBase = null) => _fields.ReadLoginOpt(cloginBase);

    public int? ReadChannelSelectSelected() => _fields.ReadChannelSelectSelected();

    public int? ReadChannelSelectWorldItemPtr() => _fields.ReadChannelSelectWorldItemPtr();

    public bool SetChannelSelectSelected(int selected) => _fields.SetChannelSelectSelected(selected);

    public bool SetChannelSelectWorldItemPtr(int ptr) => _fields.SetChannelSelectWorldItemPtr(ptr);

    public bool EnsureChannelSelectState(int worldId, int channelId) => _fields.EnsureChannelSelectState(worldId, channelId);

    public bool EnsureContextChannelArraysFromWorldItem(int worldId) => _fields.EnsureContextChannelArraysFromWorldItem(worldId);

    public int? ReadWorldSelectWorldIdx() => _fields.ReadWorldSelectWorldIdx();

    public bool SetWorldSelectWorldIdx(int worldIdx) => _fields.SetWorldSelectWorldIdx(worldIdx);

    public bool SetRequestSent(bool value = true, IntPtr? cloginBase = null) => _fields.SetRequestSent(value, cloginBase);

    public bool? ReadRequestSent(IntPtr? cloginBase = null) => _fields.ReadRequestSent(cloginBase);

    public byte[]? ReadMemory(IntPtr address, int size) => _memory.ReadMemory(address, size);

    public bool CallSendLoginPacket(int worldId, int channelId)
    {
        if (!_process.IsAttached)
        {
            _logger?.LogError("[Rue-CMemory] Not attached to process");
            return false;
        }

        if (_resolver.CLoginBase == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-CMemory] CLogin not found - call FindCLogin first");
            return false;
        }

        var funcAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.SendLoginPacketFunc));
        if (funcAddress == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-CMemory] SendLoginPacketFuncAddress not configured");
            return false;
        }

        using var caller = new RemoteFunctionCaller(_logger, _process.Handle);
        return caller.CallSendLoginPacket(_resolver.CLoginBase, funcAddress, worldId, channelId);
    }

    public byte[]? ReadFunctionCode(IntPtr functionAddr, int size = 512)
    {
        if (!_process.IsAttached)
        {
            _logger?.LogError("[Rue-CMemory] Not attached to process");
            return null;
        }

        using var caller = new RemoteFunctionCaller(_logger, _process.Handle);
        return caller.ReadFunctionBytes(functionAddr, size);
    }

    public string? DisassembleFunction(IntPtr functionAddr, int size = 256)
    {
        var bytes = ReadFunctionCode(functionAddr, size);
        if (bytes == null)
            return null;

        return RemoteFunctionCaller.DisassembleBasic(bytes, functionAddr);
    }

    public void Dispose()
    {
        Detach();
    }
}
