using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketCheckUserLimitPlug : IPipelinePlug<UserOnPacketCheckUserLimit>
{
    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckUserLimit message)
    {
        using var packet = new PacketWriter(PacketSendOperations.CheckUserLimitResult);

        packet.WriteByte(0);
        packet.WriteByte(0);

        await message.User.Dispatch(packet.Build());
    }
}
