using System.Net;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class SelectCharacterHandler : AbstractLoginPacketHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IServerService _servers;

    public SelectCharacterHandler(IServerService servers, ICharacterRepository characterRepository)
    {
        _servers = servers;
        _characterRepository = characterRepository;
    }

    public override short Operation => 108;

    public override async Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var characterID = reader.ReadInt();

        try
        {
            var character = await _characterRepository.RetrieveByAccountWorldAndCharacter(
                user.AccountWorld!.ID,
                characterID
            );
            var response = await _servers.GetGameByWorldAndChannel(new ServerGetGameByWorldAndChannelRequest(
                (int)user.SelectedWorldID!,
                (int)user.SelectedChannelID!
            ));

            if (character == null)
            {
                await user.Dispatch(new PacketWriter()
                    .WriteShort(7)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.NotAuthorized)
                );
                return;
            }

            if (response.Result != ServerResult.Success || response.Server == null)
            {
                await user.Dispatch(new PacketWriter()
                    .WriteShort(7)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.NotConnectableWorld)
                );
                return;
            }

            var gameEndpoint = new IPEndPoint(IPAddress.Parse("8.31.99.141"), response.Server.Port);
            var gameAddress = gameEndpoint.Address.MapToIPv4().GetAddressBytes();
            var chatEndpoint = new IPEndPoint(IPAddress.Parse("8.31.99.131"), 1337);
            var chatAddress = gameEndpoint.Address.MapToIPv4().GetAddressBytes();
            var packet = new PacketWriter();

            packet.WriteShort(7);

            user.Character = character;

            packet.WriteByte(0);
            packet.WriteByte(0);

            foreach (var b in gameAddress)
                packet.WriteByte(b);
            packet.WriteShort((short)gameEndpoint.Port);

            foreach (var b in chatAddress)
                packet.WriteByte(b);
            packet.WriteShort((short)chatEndpoint.Port);

            packet.WriteInt(character.ID);
            packet.WriteByte(0);
            packet.WriteInt(0);
            packet.WriteByte(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte(0);

            await user.OnMigrateOut(response.Server.ID);
            await user.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter();

            packet.WriteShort(7);

            packet.WriteBool(false);
            packet.WriteByte((byte)LoginResult.DBFail);

            await user.Dispatch(packet);
        }
    }
}
