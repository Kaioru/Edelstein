using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserEmotionHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserEmotion> _pipeline;

    public UserEmotionHandler(IPipeline<IUserEmotion> pipeline) => _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserEmotion;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var message = new UserEmotion(
            user,
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadBool()
        );

        return _pipeline.Process(message);
    }
}
