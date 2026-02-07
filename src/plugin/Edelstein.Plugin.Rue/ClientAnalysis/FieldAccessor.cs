using Edelstein.Plugin.Rue.ClientAnalysis.NativeTypes;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class FieldAccessor(
    MemoryAccessor memory,
    SingletonResolver resolver,
    NativeAllocator allocator,
    ILogger? logger)
{
    private readonly MemoryAccessor _memory = memory;
    private readonly SingletonResolver _resolver = resolver;
    private readonly NativeAllocator _allocator = allocator;
    private readonly ILogger? _logger = logger;
    private IntPtr _channelNameArrayPtr = IntPtr.Zero;
    private IntPtr _adultChannelArrayPtr = IntPtr.Zero;

    private const int MaxWorldItems = 64;
    private const int MaxChannelItems = 60;
    private const int MaxWorldId = 100;
    private const int MaxChannelId = 40;

    public bool SetWorldAndChannel(int worldId, int channelId, IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;

        if (baseAddr == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-CMemory] CWvsContext base address not set");
            return false;
        }

        var worldIdAddr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.WorldId);
        var channelIdAddr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.ChannelId);

        if (!_memory.WriteInt32WithVerification(worldIdAddr, worldId))
            return false;

        if (!_memory.WriteInt32WithVerification(channelIdAddr, channelId))
            return false;

        _logger?.LogInformation(
            "[Rue-CMemory] Write CWvsContext.m_nWorldID=0x{WorldID:X} at 0x{WorldAddr:X8} | m_nChannelID=0x{ChannelID:X} at 0x{ChannelAddr:X8}",
            worldId,
            worldIdAddr.ToInt32(),
            channelId,
            channelIdAddr.ToInt32());

        return true;
    }

    public bool SetQuestManWorldId(int worldId)
    {
        var ptrAddress = new IntPtr(unchecked((int)V95ClientStructs.Addresses.CQuestManSingletonPtr));
        if (!_memory.TryReadInt32(ptrAddress, out var questManPtr))
        {
            _logger?.LogError("[Rue-CMemory] Failed to read CQuestMan pointer");
            return false;
        }

        var questManBase = new IntPtr(questManPtr);
        if (questManBase == IntPtr.Zero)
        {
            _logger?.LogWarning("[Rue-CMemory] CQuestMan instance is NULL — not yet created");
            return false;
        }

        var worldIdAddr = IntPtr.Add(questManBase, V95ClientStructs.Offsets.CQuestMan.WorldId);
        if (!_memory.WriteInt32WithVerification(worldIdAddr, worldId))
            return false;

        _logger?.LogInformation("[Rue-CMemory] Write CQuestMan.m_nWorldID=0x{WorldId:X} at 0x{Addr:X8}",
            worldId, worldIdAddr.ToInt32());

        return true;
    }

    public int? ReadWorldId(IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.WorldId);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadChannelId(IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.ChannelId);
        return _memory.ReadInt32Stable(addr);
    }

    public uint? ReadAccountId(IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.AccountId);
        return _memory.ReadUInt32Stable(addr);
    }

    public int? ReadLoginStep(IntPtr? cloginBase = null)
    {
        var baseAddr = cloginBase ?? _resolver.CLoginBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CLogin.LoginStep);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadStepChanging(IntPtr? cloginBase = null)
    {
        var baseAddr = cloginBase ?? _resolver.CLoginBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CLogin.StepChanging);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadCharacterCount(IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.CharacterCount);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadSlotCount(IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.SlotCount);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadChannelNameArrayPtr(IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.ChannelName);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadAdultChannelArrayPtr(IntPtr? cwvsContextBase = null)
    {
        var baseAddr = cwvsContextBase ?? _resolver.CWvsContextBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CWvsContext.AdultChannel);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadCharSelected(IntPtr? cloginBase = null)
    {
        var baseAddr = cloginBase ?? _resolver.CLoginBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CLogin.CharSelected);
        return _memory.ReadInt32Stable(addr);
    }

    public byte? ReadLoginOpt(IntPtr? cloginBase = null)
    {
        var baseAddr = cloginBase ?? _resolver.CLoginBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CLogin.LoginOpt);
        return _memory.ReadByte(addr);
    }

    public bool FindCUIChannelSelect() => _resolver.FindCUIChannelSelect();

    public int? ReadChannelSelectSelected()
    {
        if (!FindCUIChannelSelect())
            return null;

        var baseAddr = _resolver.CUIChannelSelectBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CUIChannelSelect.Select);
        return _memory.ReadInt32Stable(addr);
    }

    public int? ReadChannelSelectWorldItemPtr()
    {
        if (!FindCUIChannelSelect())
            return null;

        var baseAddr = _resolver.CUIChannelSelectBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CUIChannelSelect.WorldItem);
        return _memory.ReadInt32Stable(addr);
    }

    public bool SetChannelSelectSelected(int selected)
    {
        if (!FindCUIChannelSelect())
            return false;

        var baseAddr = _resolver.CUIChannelSelectBase;
        if (baseAddr == IntPtr.Zero)
            return false;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CUIChannelSelect.Select);
        if (!_memory.WriteInt32WithVerification(addr, selected))
            return false;

        _logger?.LogInformation("[Rue-CMemory] Write CUIChannelSelect.m_nSelect=0x{Selected:X} at 0x{Addr:X8}",
            selected, addr.ToInt32());

        return true;
    }

    public bool SetChannelSelectWorldItemPtr(int ptr)
    {
        if (!FindCUIChannelSelect())
            return false;

        var baseAddr = _resolver.CUIChannelSelectBase;
        if (baseAddr == IntPtr.Zero)
            return false;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CUIChannelSelect.WorldItem);
        if (!_memory.WriteInt32WithVerification(addr, ptr))
            return false;

        _logger?.LogInformation("[Rue-CMemory] Write CUIChannelSelect.m_pWorldItem=0x{Ptr:X} at 0x{Addr:X8}",
            ptr, addr.ToInt32());

        return true;
    }

    public bool EnsureChannelSelectState(int worldId, int channelId)
    {
        if (!_resolver.FindCLogin())
            return false;

        if (!FindCUIChannelSelect())
            return false;

        if (!TryResolveWorldItemInternal(worldId, out var worldItemPtr, out _, out _))
            return false;

        if (!SetChannelSelectWorldItemPtr(worldItemPtr.ToInt32()))
            return false;

        return SetChannelSelectSelected(channelId);
    }

    public bool EnsureContextChannelArraysFromWorldItem(int worldId)
    {
        if (!_resolver.FindCWvsContext())
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: CWvsContext not found");
            return false;
        }

        if (!_resolver.FindCLogin())
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: CLogin not found");
            return false;
        }

        if (!TryResolveWorldItemInternal(worldId, out var worldItemPtr, out var channelItemsPtr, out var channelCount))
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: world item not resolved (worldId={WorldId})", worldId);
            return false;
        }

        if (channelCount <= 0)
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: channelCount={Count} (worldId={WorldId}, ci=0x{Ptr:X8})",
                channelCount, worldId, channelItemsPtr.ToInt32());
            return false;
        }

        var channelNames = new List<ZXString>(channelCount);
        var adultChannels = new List<int>(channelCount);

        for (var i = 0; i < channelCount; i++)
        {
            var baseAddr = IntPtr.Add(channelItemsPtr, i * V95ClientStructs.Offsets.ChannelItem.Stride);
            var namePtr = _memory.ReadInt32Stable(baseAddr) ?? 0;
            var adult = _memory.ReadInt32Stable(IntPtr.Add(baseAddr, V95ClientStructs.Offsets.ChannelItem.AdultFlag)) ?? 0;

            channelNames.Add(ZXString.FromPointer(new IntPtr(namePtr)));
            adultChannels.Add(adult);
        }

        var sampleCount = Math.Min(channelCount, 6);
        if (sampleCount > 0)
        {
            var samples = new List<string>(sampleCount);
            for (var i = 0; i < sampleCount; i++)
            {
                var itemPtr = IntPtr.Add(channelItemsPtr, i * V95ClientStructs.Offsets.ChannelItem.Stride);
                var namePtr = _memory.ReadInt32Stable(itemPtr) ?? 0;
                var name = namePtr != 0 ? _memory.ReadCString(new IntPtr(namePtr), 48) : string.Empty;
                var adult = adultChannels[i];
                var label = string.IsNullOrWhiteSpace(name) ? "<null>" : name;
                samples.Add($"#{i} name={label}(0x{namePtr:X}) adult={adult}");
            }

            _logger?.LogInformation(
                "[Rue-CMemory] Channel items resolved: worldId={WorldId}, worldItem=0x{WorldItem:X8}, items=0x{Items:X8}, count={Count}, sample=[{Sample}]",
                worldId,
                worldItemPtr.ToInt32(),
                channelItemsPtr.ToInt32(),
                channelCount,
                string.Join(", ", samples));
        }

        var channelNameArray = ZArray<ZXString>.Create(_allocator, channelNames);
        if (channelNameArray == IntPtr.Zero)
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: create channel name ZArray");
            return false;
        }

        var adultChannelArray = ZArray<int>.Create(_allocator, adultChannels);
        if (adultChannelArray == IntPtr.Zero)
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: create adult channel ZArray");
            return false;
        }

        if (_channelNameArrayPtr != IntPtr.Zero)
            _allocator.Free(IntPtr.Subtract(_channelNameArrayPtr, TypeSizes.Int32));
        if (_adultChannelArrayPtr != IntPtr.Zero)
            _allocator.Free(IntPtr.Subtract(_adultChannelArrayPtr, TypeSizes.Int32));

        _channelNameArrayPtr = channelNameArray;
        _adultChannelArrayPtr = adultChannelArray;

        var cwvsChannelNameAddr = IntPtr.Add(_resolver.CWvsContextBase, V95ClientStructs.Offsets.CWvsContext.ChannelName);
        var cwvsAdultChannelAddr = IntPtr.Add(_resolver.CWvsContextBase, V95ClientStructs.Offsets.CWvsContext.AdultChannel);

        if (!_memory.WriteInt32WithVerification(cwvsChannelNameAddr, channelNameArray.ToInt32()))
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: write CWvsContext ChannelName ptr");
            return false;
        }

        _logger?.LogInformation(
            "[Rue-CMemory] Write CWvsContext.m_aChannelName.a count={Count} ptr=0x{Ptr:X8} at 0x{Addr:X8}",
            channelCount,
            channelNameArray.ToInt32(),
            cwvsChannelNameAddr.ToInt32());

        if (!_memory.WriteInt32WithVerification(cwvsAdultChannelAddr, adultChannelArray.ToInt32()))
        {
            _logger?.LogWarning("[Rue-CMemory] EnsureContextChannelArraysFromWorldItem failed: write CWvsContext AdultChannel ptr");
            return false;
        }

        _logger?.LogInformation(
            "[Rue-CMemory] Write CWvsContext.m_aAdultChannel.a count={Count} ptr=0x{Ptr:X8} at 0x{Addr:X8}",
            channelCount,
            adultChannelArray.ToInt32(),
            cwvsAdultChannelAddr.ToInt32());

        return true;
    }

    private bool TryResolveWorldItemInternal(int worldId, out IntPtr worldItemPtr, out IntPtr channelItemsPtr, out int channelCount)
    {
        worldItemPtr = IntPtr.Zero;
        channelItemsPtr = IntPtr.Zero;
        channelCount = 0;

        if (_resolver.CLoginBase == IntPtr.Zero)
            return false;

        var worldArrayPtrValue = _memory.ReadInt32Stable(IntPtr.Add(_resolver.CLoginBase, V95ClientStructs.Offsets.CLogin.WorldItem));
        if (!worldArrayPtrValue.HasValue || worldArrayPtrValue.Value == 0)
            return false;

        var worldArrayPtr = new IntPtr(worldArrayPtrValue.Value);

        for (var i = 0; i < MaxWorldItems; i++)
        {
            var itemPtr = IntPtr.Add(worldArrayPtr, i * V95ClientStructs.Offsets.WorldItem.Stride);
            if (!_memory.IsReadable(itemPtr, V95ClientStructs.Offsets.WorldItem.Stride))
                break;

            var id = _memory.ReadInt32Stable(IntPtr.Add(itemPtr, V95ClientStructs.Offsets.WorldItem.Id));
            if (!id.HasValue)
                break;

            if (id.Value < 0 || id.Value > MaxWorldId)
                break;

            var namePtr = _memory.ReadInt32Stable(IntPtr.Add(itemPtr, V95ClientStructs.Offsets.WorldItem.NamePtr));
            if (namePtr.HasValue && namePtr.Value != 0)
            {
                if (!_memory.IsReadable(new IntPtr(namePtr.Value), 1))
                    break;
            }

            if (id.Value != worldId)
                continue;

            worldItemPtr = itemPtr;
            var ciPtrValue = _memory.ReadInt32Stable(IntPtr.Add(itemPtr, V95ClientStructs.Offsets.WorldItem.ChannelItemsPtr));
            channelItemsPtr = ciPtrValue.HasValue ? new IntPtr(ciPtrValue.Value) : IntPtr.Zero;
            channelCount = ScanChannelCount(channelItemsPtr, worldId);
            return true;
        }

        return false;
    }

    private int ScanChannelCount(IntPtr channelItemsPtr, int worldId)
    {
        if (channelItemsPtr == IntPtr.Zero)
            return 0;

        var count = 0;

        for (var i = 0; i < MaxChannelItems; i++)
        {
            var chPtr = IntPtr.Add(channelItemsPtr, i * V95ClientStructs.Offsets.ChannelItem.Stride);
            if (!_memory.IsReadable(chPtr, V95ClientStructs.Offsets.ChannelItem.Stride))
                break;

            var namePtr = _memory.ReadInt32Stable(IntPtr.Add(chPtr, V95ClientStructs.Offsets.ChannelItem.NamePtr));
            if (namePtr.HasValue && namePtr.Value != 0)
            {
                if (!_memory.IsReadable(new IntPtr(namePtr.Value), 1))
                    break;
            }

            var chWorld = _memory.ReadInt32Stable(IntPtr.Add(chPtr, V95ClientStructs.Offsets.ChannelItem.WorldId));
            var chId = _memory.ReadInt32Stable(IntPtr.Add(chPtr, V95ClientStructs.Offsets.ChannelItem.ChannelId));
            if (!chWorld.HasValue || !chId.HasValue)
                break;

            if (chWorld.Value != worldId)
                break;

            if (chId.Value < 0 || chId.Value > MaxChannelId)
                break;

            count++;
        }

        return count;
    }

    public bool? IsConnectionDlgValid()
    {
        if (!FindCUIChannelSelect())
            return null;

        var baseAddr = _resolver.CUIChannelSelectBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CUIChannelSelect.ConnectionDlg);
        if (!_memory.TryReadInt32(addr, out var ptr))
            return null;

        return ptr != 0;
    }

    public bool FindCUIWorldSelect() => _resolver.FindCUIWorldSelect();

    public int? ReadWorldSelectWorldIdx()
    {
        if (!FindCUIWorldSelect())
            return null;

        var baseAddr = _resolver.CUIWorldSelectBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CUIWorldSelect.WorldIdx);
        return _memory.ReadInt32Stable(addr);
    }

    public bool SetWorldSelectWorldIdx(int worldIdx)
    {
        if (!FindCUIWorldSelect())
        {
            _logger?.LogError("[Rue-CMemory] CUIWorldSelect not found");
            return false;
        }

        var baseAddr = _resolver.CUIWorldSelectBase;
        if (baseAddr == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-CMemory] CUIWorldSelect base is NULL");
            return false;
        }

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CUIWorldSelect.WorldIdx);
        if (!_memory.WriteInt32WithVerification(addr, worldIdx))
            return false;

        _logger?.LogInformation("[Rue-CMemory] Write CUIWorldSelect.m_nWorldIdx=0x{WorldIdx:X} at 0x{Addr:X8}",
            worldIdx, addr.ToInt32());

        return true;
    }

    public bool SetRequestSent(bool value = true, IntPtr? cloginBase = null)
    {
        var baseAddr = cloginBase ?? _resolver.CLoginBase;
        if (baseAddr == IntPtr.Zero)
        {
            _logger?.LogError("[Rue-CMemory] CLogin base address not set");
            return false;
        }

        var stepChanging = ReadStepChanging(baseAddr);
        if (stepChanging is > 0)
        {
            _logger?.LogWarning("[Rue-CMemory] Refusing to write m_bRequestSent while m_tStepChanging={Value}", stepChanging);
            return false;
        }

        var requestSentAddr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CLogin.RequestSent);
        if (!_memory.WriteInt32WithVerification(requestSentAddr, value ? 1 : 0))
            return false;

        _logger?.LogInformation("[Rue-CMemory] Write CLogin.m_bRequestSent={Value} at 0x{Addr:X8}",
            value ? 1 : 0, requestSentAddr.ToInt32());

        return true;
    }

    public bool? ReadRequestSent(IntPtr? cloginBase = null)
    {
        var baseAddr = cloginBase ?? _resolver.CLoginBase;
        if (baseAddr == IntPtr.Zero)
            return null;

        var addr = IntPtr.Add(baseAddr, V95ClientStructs.Offsets.CLogin.RequestSent);
        var value = _memory.ReadInt32Stable(addr);
        return value.HasValue ? value.Value != 0 : null;
    }
}
