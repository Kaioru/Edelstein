using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SelectWorldPlug : IPipelinePlug<ISelectWorld>
{
    private readonly IAccountWorldRepository _accountWorldRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IServerService _server;

    public SelectWorldPlug(
        IServerService server,
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository
    )
    {
        _server = server;
        _accountWorldRepository = accountWorldRepository;
        _characterRepository = characterRepository;
    }

    public async Task Handle(IPipelineContext ctx, ISelectWorld message)
    {
        try
        {
            var gameStage = await _server.GetGameByWorldAndChannel(
                new ServerGetGameByWorldAndChannelRequest(
                    message.WorldID,
                    message.ChannelID
                )
            );
            var result = gameStage.Result == ServerResult.Success || gameStage.Server == null
                ? LoginResult.Success
                : LoginResult.DBFail;
            var accountWorld = await _accountWorldRepository.RetrieveByAccountAndWorld(
                message.User.Account!.ID,
                gameStage.Server!.WorldID
            ) ?? await _accountWorldRepository.Insert(new AccountWorld
            {
                AccountID = message.User.Account.ID,
                WorldID = gameStage.Server!.WorldID
            });
            var characters = (await _characterRepository
                    .RetrieveAllByAccountWorld(accountWorld.ID))
                .ToImmutableList();
            var packet = new PacketWriter(PacketSendOperations.SelectWorldResult);

            packet.WriteByte((byte)result);

            if (result == LoginResult.Success)
            {
                message.User.State = LoginState.SelectCharacter;
                message.User.AccountWorld = accountWorld;
                message.User.SelectedWorldID = (byte)gameStage.Server!.WorldID;
                message.User.SelectedChannelID = (byte)gameStage.Server!.ChannelID;

                message.User.Account.LatestConnectedWorld = message.User.SelectedWorldID;

                packet.WriteByte((byte)characters.Count);

                foreach (var character in characters)
                {
                    packet.WriteCharacterStats(character);
                    packet.WriteCharacterLooks(character);

                    packet.WriteBool(false);
                    packet.WriteBool(false);
                }

                packet.WriteBool(!string.IsNullOrEmpty(message.User.Account.SPW));
                packet.WriteInt(accountWorld.CharacterSlotMax);
                packet.WriteInt(0);
            }

            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter(PacketSendOperations.SelectWorldResult);

            packet.WriteByte((byte)LoginResult.DBFail);

            await message.User.Dispatch(packet);
        }
    }
}
