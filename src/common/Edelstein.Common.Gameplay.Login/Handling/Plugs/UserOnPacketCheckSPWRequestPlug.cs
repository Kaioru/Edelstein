﻿using System.Net;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Plugs;

public class UserOnPacketCheckSPWRequestPlug : IPipelinePlug<UserOnPacketCheckSPWRequest>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IServerService _serverService;

    public UserOnPacketCheckSPWRequestPlug(
        ICharacterRepository characterRepository,
        IServerService serverService
    )
    {
        _characterRepository = characterRepository;
        _serverService = serverService;
    }

    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckSPWRequest message)
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
                using var failedPacket = new PacketWriter(PacketSendOperations.CheckSPWResult)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.NotAuthorized);
                await message.User.Dispatch(failedPacket.Build());
                return;
            }

            if (response.Result != ServerResult.Success || response.Server == null)
            {
                using var failedPacket = new PacketWriter(PacketSendOperations.CheckSPWResult)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.NotConnectableWorld);
                await message.User.Dispatch(failedPacket.Build());
                return;
            }

            if (!BCrypt.Net.BCrypt.Verify(message.SPW, message.User.Account!.SPW))
            {
                using var failedPacket = new PacketWriter(PacketSendOperations.CheckSPWResult)
                    .WriteBool(false)
                    .WriteByte((byte)LoginResult.IncorrectSPW);
                await message.User.Dispatch(failedPacket.Build());
                return;
            }

            var endpoint = new IPEndPoint(IPAddress.Parse(response.Server.Host), response.Server.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = endpoint.Port;
            using var packet = new PacketWriter(PacketSendOperations.SelectCharacterResult);

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
            using var packet = new PacketWriter(PacketSendOperations.CheckSPWResult);

            packet.WriteBool(false);
            packet.WriteByte((byte)LoginResult.DBFail);

            await message.User.Dispatch(packet.Build());
        }
    }
}
