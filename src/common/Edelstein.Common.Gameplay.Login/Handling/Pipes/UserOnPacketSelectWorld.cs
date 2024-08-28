using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Shared;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Shared;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketSelectWorld : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, SelectWorld>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, SelectWorld> message)
    {
        await message.User.Dispatch(new SelectWorldResult
        {
            Result = LoginResultCode.Success,
            Info = new SelectWorldResultSuccessInfo
            {
                Characters = new List<SelectWorldResultSuccessInfoCharacter>
                {
                    new()
                    {
                        CharacterStat = new StructuredCharacterStat
                        {
                            ID = 1,
                            Name = "Beef",
                            Hair = 30000,
                            Face = 20724
                        },
                        AvatarLook = new StructuredAvatarLook
                        {
                            Hair = 30000,
                            Face = 20724
                        }
                    }
                },
                SlotCount = 3
            }
        });
    }
}
