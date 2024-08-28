using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Shared;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketSelectWorld(
    IServerService servers,
    IAccountWorldDataRepository accountWorldDataRepository,
    ICharacterRepository characterRepository
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, SelectWorld>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, SelectWorld> message)
    {
        if (message.User.State != LoginState.SelectWorld) return;

        try
        {
            var response = await servers.GetGameByWorldAndChannel(new ServerServiceGetByWorldAndChannelRequest
            {
                WorldID = message.Packet.WorldID,
                ChannelID = message.Packet.ChannelID
            });
            var accountWorldData = await accountWorldDataRepository.RetrieveByAccountAndWorld(message.User.Account!.ID, response.Info!.WorldID) ??
                                   await accountWorldDataRepository.Insert(new AccountWorldData
                                   {
                                       AccountID = message.User.Account.ID,
                                       WorldID = response.Info.WorldID
                                   });
            var characters = await characterRepository.RetrieveAllByAccountWorldData(accountWorldData.ID);

            message.User.AccountWorldData = accountWorldData;
            message.User.State = LoginState.SelectCharacter;
            message.User.SelectedWorldID = (byte)response.Info!.WorldID;
            message.User.SelectedChannelID = (byte)response.Info!.ChannelID;

            await message.User.Dispatch(new SelectWorldResult
            {
                Result = LoginResultCode.Success,
                Info = new SelectWorldResultSuccessInfo
                {
                    Characters = characters
                        .Select(c => new SelectWorldResultSuccessInfoCharacter
                        {
                            CharacterStat = c.ToStructuredCharacterStat(),
                            AvatarLook = c.ToStructuredAvatarLook()
                        })
                        .ToList(),
                    SlotCount = accountWorldData.CharacterSlotMax
                }
            });
        }
        catch (Exception)
        {
            await message.User.Dispatch(new SelectWorldResult
            {
                Result = LoginResultCode.Unknown
            });
        }
    }
}
