using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Network.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CheckUserLimitPlug : IPipelinePlug<ICheckUserLimit>
{
    public async Task Handle(IPipelineContext ctx, ICheckUserLimit message)
    {
        var packet = new PacketOut(PacketSendOperations.CheckUserLimitResult);

        packet.WriteByte(0);
        packet.WriteByte(0);

        await message.User.Dispatch(packet);
    }
}
