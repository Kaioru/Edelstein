using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SPWChangePlug : IPipelinePlug<ISPWChange>
{
    public async Task Handle(IPipelineContext ctx, ISPWChange message)
    {
        if (!BCrypt.Net.BCrypt.Verify(message.SPWCurrent, message.User.Account!.SPW))
        {
            await message.User.Dispatch(
                new PacketWriter(PacketSendOperations.ChangeSPWResult)
                    .WriteByte((byte)LoginResult.IncorrectSPW)
            );
            return;
        }

        message.User.Account!.SPW = BCrypt.Net.BCrypt.HashPassword(message.SPWNew);
        await message.User.Dispatch(
            new PacketWriter(PacketSendOperations.ChangeSPWResult)
                .WriteByte((byte)LoginResult.Success)
        );
    }
}
