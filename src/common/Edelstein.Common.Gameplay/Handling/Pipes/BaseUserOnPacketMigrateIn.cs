using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Handling.Pipes;

public class BaseUserOnPacketMigrateIn<TStageSystem, TStageSystemUser>(
    IMigrationService migrations,
    ISessionService sessions
) : IPipe<PipedPacketMessage<TStageSystem, TStageSystemUser, MigrateIn>>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : class, IStageSystemUser<TStageSystem, TStageSystemUser>
{
    public async Task Handle(IPipelineContext ctx, PipedPacketMessage<TStageSystem, TStageSystemUser, MigrateIn> message)
    {
        Console.WriteLine("HEY");
        if (message.User.Account != null || 
            message.User.AccountWorldData != null || 
            message.User.Character != null)
        {
            await message.User.Disconnect();
            return;
        }

        try
        {
            var migrationResponse = await migrations.Claim(new MigrationServiceClaimRequest
            {
                CharacterID = message.Packet.CharacterID,
                ServerID = message.User.System.ID,
                Secret = message.Packet.ClientKey
            });
            Console.WriteLine(migrationResponse);
            var sessionServerResponse = await sessions.UpdateServer(new SessionServiceUpdateServerRequest
            {
                AccountID = migrationResponse.Info!.AccountID,
                ServerID = message.User.System.ID,
                Secret = message.Packet.ClientKey
            });
            var sessionCharacterResponse = await sessions.UpdateCharacter(new SessionServiceUpdateCharacterRequest
            {
                AccountID = migrationResponse.Info!.AccountID,
                CharacterID = migrationResponse.Info!.CharacterID,
                Secret = message.Packet.ClientKey
            });
            
            if (sessionServerResponse.Result != SessionServiceResult.Success ||
                sessionCharacterResponse.Result != SessionServiceResult.Success)
                await message.User.Disconnect();
            
            message.User.Account = migrationResponse.Info.AccountSnapshot.Value;
            message.User.AccountWorldData = migrationResponse.Info.AccountWorldDataSnapshot.Value;
            message.User.Character = migrationResponse.Info.CharacterSnapshot.Value;
            message.User.Key = message.Packet.ClientKey;
            
            
            Console.WriteLine(message.User.Account);
        }
        catch
        {
            await message.User.Disconnect();
        }
    }
}
