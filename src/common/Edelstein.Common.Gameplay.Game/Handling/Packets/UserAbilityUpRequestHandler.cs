using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserAbilityUpRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserAbilityUpRequest>
{
    public override short Operation => (short)PacketRecvOperations.UserAbilityUpRequest;

    public UserAbilityUpRequestHandler(IPipeline<FieldOnPacketUserAbilityUpRequest> pipeline) : base(pipeline)
    {
    }

    protected override FieldOnPacketUserAbilityUpRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user, 
            (ModifyStatType)reader.Skip(4).ReadInt()
        );
}
