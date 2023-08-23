using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserEmotionHandler : AbstractFieldHandler
{
    private readonly IPipeline<FieldOnPacketUserEmotion> _pipeline;

    public UserEmotionHandler(IPipeline<FieldOnPacketUserEmotion> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserEmotion;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var message = new FieldOnPacketUserEmotion(
            user,
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadBool()
        );

        return _pipeline.Process(message);
    }
}
