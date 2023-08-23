using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserEmotionPlug : IPipelinePlug<FieldOnPacketUserEmotion>
{
    public Task Handle(IPipelineContext ctx, FieldOnPacketUserEmotion message)
    {
        using var packet = new PacketWriter(PacketSendOperations.UserEmotion);

        packet.WriteInt(message.User.Character.ID);
        packet.WriteInt(message.Emotion);
        packet.WriteInt(message.Duration);
        packet.WriteBool(message.ByItemOption);

        return message.User.FieldSplit!.Dispatch(packet.Build(), message.User);
    }
}
