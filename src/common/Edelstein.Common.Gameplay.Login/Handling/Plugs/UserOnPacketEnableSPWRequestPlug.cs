using System.Net;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Plugs;

public class UserOnPacketEnableSPWRequestPlug : IPipelinePlug<UserOnPacketEnableSPWRequest>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IServerService _serverService;

    public UserOnPacketEnableSPWRequestPlug(
        IServerService serverService,
        ICharacterRepository characterRepository
    )
    {
        _serverService = serverService;
        _characterRepository = characterRepository;
    }

    public async Task Handle(IPipelineContext ctx, UserOnPacketEnableSPWRequest message)
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
                    .Build()
                );
                return;
            }

            if (response.Result != ServerResult.Success || response.Server == null)
            {
                await message.User.Dispatch(new PacketWriter(PacketSendOperations.EnableSPWResult)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.NotConnectableWorld)
                    .Build()
                );
                return;
            }

            var endpoint = new IPEndPoint(IPAddress.Parse(response.Server.Host), response.Server.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = endpoint.Port;
            using var packet = new PacketWriter(PacketSendOperations.SelectCharacterResult);

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

            await message.User.Migrate(response.Server.ID, packet.Build());
        }
        catch (Exception)
        {
            using var packet = new PacketWriter(PacketSendOperations.EnableSPWResult);

            packet.WriteBool(false);
            packet.WriteByte((byte)LoginResult.DBFail);

            await message.User.Dispatch(packet.Build());
        }
    }
}
