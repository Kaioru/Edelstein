using System.Net;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CharacterSelectPlug : IPipelinePlug<ICharacterSelect>
{
    private readonly IServerService _servers;
    private readonly ICharacterRepository _characters;

    public CharacterSelectPlug(
        IServerService servers,
        ICharacterRepository characters
    )
    {
        _servers = servers;
        _characters = characters;
    }

    public async Task Handle(IPipelineContext ctx, ICharacterSelect message)
    {
        try
        {
            var character = await _characters.RetrieveByAccountWorldAndCharacter(
                message.User.AccountWorld!.ID,
                message.CharacterID
            );
            var gameResponse = await _servers.GetGameByWorldAndChannel(new ServerGetGameByWorldAndChannelRequest(
                (int)message.User.SelectedWorldID!,
                (int)message.User.SelectedChannelID!
            ));
            var chatResponse = await _servers.GetChatByWorld(new ServerGetChatByWorldRequest(
                (int)message.User.SelectedWorldID!
            ));

            if (character == null)
            {
                await message.User.Dispatch(new PacketWriter(PacketSendOperations.SelectCharacterResult)
                    .WriteByte((byte)LoginResult.NotAuthorized)
                    .WriteBool(false)
                );
                return;
            }

            if (gameResponse.Result != ServerResult.Success || gameResponse.Server == null ||
                chatResponse.Result != ServerResult.Success || !chatResponse.Servers.Any())
            {
                await message.User.Dispatch(new PacketWriter(PacketSendOperations.SelectCharacterResult)
                    .WriteByte((byte)LoginResult.NotConnectableWorld)
                    .WriteBool(false)
                );
                return;
            }

            var gameEndpoint = new IPEndPoint(IPAddress.Parse("8.31.99.141"), gameResponse.Server.Port);
            var gameAddress = gameEndpoint.Address.MapToIPv4().GetAddressBytes();
            var chatEndpoint = new IPEndPoint(IPAddress.Parse("8.31.99.131"), chatResponse.Servers.First().Port);
            var chatAddress = gameEndpoint.Address.MapToIPv4().GetAddressBytes();
            var packet = new PacketWriter(PacketSendOperations.SelectCharacterResult);

            message.User.Character = character;

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

            await message.User.OnMigrateOut(gameResponse.Server.ID);
            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter(PacketSendOperations.SelectCharacterResult);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteBool(false);

            await message.User.Dispatch(packet);
        }
    }
}
