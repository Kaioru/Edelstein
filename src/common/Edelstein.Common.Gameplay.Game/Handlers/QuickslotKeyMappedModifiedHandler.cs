using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class QuickslotKeyMappedModifiedHandler : AbstractPipedFieldHandler<FieldOnPacketUserQuickslotKeyMappedModified>
{
    public QuickslotKeyMappedModifiedHandler(IPipeline<FieldOnPacketUserQuickslotKeyMappedModified> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.QuickslotKeyMappedModified;
    
    // TODO petconsume
    // TODO petconsumeMP
    protected override FieldOnPacketUserQuickslotKeyMappedModified? Serialize(IFieldUser user, IPacketReader reader)
    {
        var keys = new Dictionary<int, int>();
        for (var i = 0; i < 8; i++)
            keys.Add(i, reader.ReadInt());
        return new(user, keys);
    }
}
