using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketCheckDuplicatedID(
    ICharacterRepository characters
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckDuplicatedID>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckDuplicatedID> message)
    {
        if (message.User.State != LoginState.SelectCharacter) return;
        
        try
        {
            var exists = await characters.CheckExistsByName(message.Packet.CharName.Value);
            
            await message.User.Dispatch(new CheckDuplicatedIDResult
            {
                CheckedName = message.Packet.CharName,
                Reason = (byte)(exists ? 1 : 0)
            });
        }
        catch
        {
            await message.User.Dispatch(new CheckDuplicatedIDResult
            {
                CheckedName = message.Packet.CharName,
                Reason = 3
            });
        }
    }
}
