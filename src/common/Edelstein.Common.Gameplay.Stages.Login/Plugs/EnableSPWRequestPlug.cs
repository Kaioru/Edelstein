using System.Net;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class EnableSPWRequestPlug : IPipelinePlug<IEnableSPWRequest>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IMigrationService _migrationService;
    private readonly IServerService _serverService;

    public EnableSPWRequestPlug(
        IServerService serverService,
        ICharacterRepository characterRepository,
        IMigrationService migrationService
    )
    {
        _serverService = serverService;
        _characterRepository = characterRepository;
        _migrationService = migrationService;
    }

    public async Task Handle(IPipelineContext ctx, IEnableSPWRequest message)
    {
        try
        {
            var character = await _characterRepository.RetrieveByAccountWorldAndCharacter(
                message.User.AccountWorld!.ID,
                message.CharacterID
            );
            var response = await _serverService.GetGameByWorldAndChannel(new ServerGetGameByWorldAndChannelRequest(
                (int)message.User.SelectedWorldID!,
                (int)message.User.SelectedChannelID!
            ));

            if (character == null)
            {
                await message.User.Dispatch(new PacketWriter(PacketSendOperations.EnableSPWResult)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.NotAuthorized)
                );
                return;
            }

            if (response.Result != ServerResult.Success || response.Server == null)
            {
                await message.User.Dispatch(new PacketWriter(PacketSendOperations.EnableSPWResult)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.NotConnectableWorld)
                );
                return;
            }

            var endpoint = new IPEndPoint(IPAddress.Parse(response.Server.Host), response.Server.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = endpoint.Port;
            var packet = new PacketWriter(PacketSendOperations.SelectCharacterResult);

            message.User.Account!.SPW = BCrypt.Net.BCrypt.HashPassword(message.SPW);
            message.User.Character = character;

            packet.WriteByte(0);
            packet.WriteByte(0);

            foreach (var b in address)
                packet.WriteByte(b);
            packet.WriteShort((short)port);

            packet.WriteInt(character.ID);
            packet.WriteByte(0);
            packet.WriteInt(0);

            await message.User.OnMigrateOut(response.Server.ID);
            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter(PacketSendOperations.EnableSPWResult);

            packet.WriteBool(false);
            packet.WriteByte((byte)LoginResult.DBFail);

            await message.User.Dispatch(packet);
        }
    }
}
