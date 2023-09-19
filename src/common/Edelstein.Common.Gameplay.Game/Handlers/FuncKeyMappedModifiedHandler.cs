using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class FuncKeyMappedModifiedHandler : AbstractPipedFieldHandler<FieldOnPacketUserFuncKeyMappedModified>
{
    public FuncKeyMappedModifiedHandler(IPipeline<FieldOnPacketUserFuncKeyMappedModified> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.FuncKeyMappedModified;
    
    // TODO petconsume
    // TODO petconsumeMP
    protected override FieldOnPacketUserFuncKeyMappedModified? Serialize(IFieldUser user, IPacketReader reader)
    {
        var type = reader.ReadInt();
        
        if (type != 0) return null;
        
        var count = reader.ReadInt();
        var keys = new Dictionary<int, ICharacterFuncKeyRecord>();

        for (var i = 0; i < count; i++)
        {
            var key = reader.ReadInt();
            keys[key] = new CharacterFuncKeyRecord()
            {
                Type = reader.ReadByte(),
                Action = reader.ReadInt()
            };
        }

        return new(user, keys);
    }
}
