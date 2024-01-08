using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserAbilityMassUpRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserAbilityMassUpRequest>
{
    public override short Operation => (short)PacketRecvOperations.UserAbilityMassUpRequest;
    
    public UserAbilityMassUpRequestHandler(IPipeline<FieldOnPacketUserAbilityMassUpRequest> pipeline) : base(pipeline)
    {
    }

    protected override FieldOnPacketUserAbilityMassUpRequest? Serialize(IFieldUser user, IPacketReader reader)
    {
        var count = reader.Skip(4).ReadInt();
        var statUp = new Dictionary<ModifyStatType, int>();
        
        for (var i = 0; i < count; i++)
            statUp.Add(
                (ModifyStatType)reader.ReadInt(),
                reader.ReadInt()
            );
        
        return new(
            user,
            statUp
        );
    }
}
