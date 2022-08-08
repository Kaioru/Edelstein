using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserEmotionPlug : IPipelinePlug<IUserEmotion>
{
    public Task Handle(IPipelineContext ctx, IUserEmotion message)
    {
        var packet = new PacketWriter(PacketSendOperations.UserEmotion);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteInt(message.Emotion);
        packet.WriteInt(message.Duration);
        packet.WriteBool(message.ByItemOption);

        return message.User.FieldSplit!.Dispatch(packet, message.User);
    }
}
