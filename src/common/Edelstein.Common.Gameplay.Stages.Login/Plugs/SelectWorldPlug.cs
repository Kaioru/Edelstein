﻿using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Bytes;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SelectWorldPlug : IPipelinePlug<ISelectWorld>
{
    private readonly IAccountWorldRepository _repository;
    private readonly IServerService _server;

    public SelectWorldPlug(IServerService server, IAccountWorldRepository repository)
    {
        _server = server;
        _repository = repository;
    }

    public async Task Handle(IPipelineContext ctx, ISelectWorld message)
    {
        try
        {
            var gameStage = await _server.GetGameByWorldAndChannel(
                new ServerGetGameByWorldAndChannelRequest(
                    message.WorldID,
                    message.ChannelIndex + 1
                )
            );
            var result = gameStage.Result == ServerResult.Success || gameStage.Server == null
                ? LoginResult.Success
                : LoginResult.DBFail;
            var accountWorld = await _repository.RetrieveByAccountAndWorld(
                message.User.Account!.ID,
                gameStage.Server!.WorldID
            ) ?? await _repository.Insert(new AccountWorld
            {
                AccountID = message.User.Account.ID,
                WorldID = gameStage.Server!.WorldID
            });
            var packet = new ByteWriter(PacketSendOperations.SelectWorldResult);

            packet.WriteByte((byte)result);

            if (result == LoginResult.Success)
            {
                message.User.State = LoginState.SelectCharacter;
                message.User.AccountWorld = accountWorld;
                message.User.SelectedWorldID = (byte)gameStage.Server!.WorldID;
                message.User.SelectedChannelID = (byte)gameStage.Server!.ChannelID;

                message.User.Account.LatestConnectedWorld = message.User.SelectedWorldID;

                packet.WriteByte(0);
                packet.WriteBool(true);
                packet.WriteInt(accountWorld.CharacterSlotMax);
                packet.WriteInt(0);
            }

            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new ByteWriter(PacketSendOperations.SelectWorldResult);

            packet.WriteByte((byte)LoginResult.DBFail);

            await message.User.Dispatch(packet);
        }
    }
}