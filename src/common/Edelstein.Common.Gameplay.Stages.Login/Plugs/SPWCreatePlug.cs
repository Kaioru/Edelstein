using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SPWCreatePlug : IPipelinePlug<ISPWCreate>
{
    public Task Handle(IPipelineContext ctx, ISPWCreate message)
    {
        message.User.Account!.SPW = BCrypt.Net.BCrypt.HashPassword(message.SPW);

        return message.User.Dispatch(
            new PacketWriter(PacketSendOperations.ChangeSPWResult)
                .WriteByte((byte)LoginResult.Success)
        );
    }
}
