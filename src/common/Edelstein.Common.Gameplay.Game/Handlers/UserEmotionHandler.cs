using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserEmotionHandler : AbstractPipedFieldHandler<FieldOnPacketUserEmotion>
{
    public UserEmotionHandler(IPipeline<FieldOnPacketUserEmotion> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserEmotion;

    protected override FieldOnPacketUserEmotion? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user,
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadBool()
        );
}
