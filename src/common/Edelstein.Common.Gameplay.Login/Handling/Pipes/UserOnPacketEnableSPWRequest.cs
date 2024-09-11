using System.Net;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Pipes;

public class UserOnPacketEnableSPWRequest(
    ICharacterRepository characters,
    IServerService servers,
    IMigrationService migrations
) : IPipe<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, EnableSPWRequest>>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, EnableSPWRequest> message)
    {
        if (message.User.State != LoginState.SelectCharacter) return;
        if (!string.IsNullOrEmpty(message.User.Account!.SPW)) return;

        try
        {
            var character = await characters.RetrieveByAccountWorldDataAndCharacter(
                message.User.AccountWorldData!.ID,
                message.Packet.CharacterID
            );
            
            var serverResponse = await servers.GetGameByWorldAndChannel(new ServerServiceGetByWorldAndChannelRequest
            {
                WorldID = (int)message.User.SelectedWorldID!,
                ChannelID = (int)message.User.SelectedChannelID!
            });
            
            message.User.Account!.SPW = BCrypt.Net.BCrypt.EnhancedHashPassword(message.Packet.SPW.Value);
            message.User.Character = character;
            message.User.IsMigrating = true;
            
            var migrationResponse = await migrations.Start(new MigrationServiceStartRequest
            {
                Info = new MigrationInfo
                {
                    AccountID = message.User.Account.ID,
                    AccountWorldDataID = message.User.AccountWorldData.ID,
                    CharacterID = character!.ID,
                    FromServerID = message.User.System.Options.ID,
                    ToServerID = serverResponse.Info!.ID,
                    AccountSnapshot = new MigrationInfoSnapshot<Account>(message.User.Account),
                    AccountWorldDataSnapshot = new MigrationInfoSnapshot<AccountWorldData>(message.User.AccountWorldData),
                    CharacterSnapshot = new MigrationInfoSnapshot<Character>(character)
                }
            });

            if (migrationResponse.Result != MigrationServiceResult.Success)
            {
                await message.User.Dispatch(new EnableSPWResult
                {
                    Result = LoginSPWResultCode.Unknown
                });
                return;
            }
            
            var endpoint = new IPEndPoint(IPAddress.Parse(serverResponse.Info.Host), serverResponse.Info.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = (short)endpoint.Port;

            await message.User.Dispatch(new SelectCharacterResult
            {
                Result = LoginResultCode.Success,
                Info = new SelectCharacterResultInfo
                {
                    Address = address,
                    Port = port,
                    CharacterID = character.ID
                }
            });
        }
        catch
        {
            await message.User.Dispatch(new EnableSPWResult
            {
                Result = LoginSPWResultCode.Unknown
            });
        }
    }
}
