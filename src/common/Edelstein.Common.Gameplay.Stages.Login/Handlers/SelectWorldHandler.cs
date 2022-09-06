using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Accounts;
using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class SelectWorldHandler : AbstractLoginPacketHandler
{
    private readonly IAccountWorldRepository _accountWorldRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IServerService _server;

    public SelectWorldHandler(
        IAccountWorldRepository accountWorldRepository,
        ICharacterRepository characterRepository,
        IServerService server
    )
    {
        _accountWorldRepository = accountWorldRepository;
        _characterRepository = characterRepository;
        _server = server;
    }

    public override short Operation => (short)PacketRecvOperations.SelectWorld;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectWorld;

    public override async Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        _ = reader.ReadByte();
        var worldID = reader.ReadByte();
        var channelID = reader.ReadByte();

        try
        {
            var gameStage = await _server.GetGameByWorldAndChannel(
                new ServerGetGameByWorldAndChannelRequest(
                    worldID,
                    channelID
                )
            );
            var result = gameStage.Result == ServerResult.Success && gameStage.Server != null
                ? LoginResult.Success
                : LoginResult.DBFail;
            var accountWorld = await _accountWorldRepository.RetrieveByAccountAndWorld(
                user.Account!.ID,
                gameStage.Server!.WorldID
            ) ?? await _accountWorldRepository.Insert(new AccountWorld
            {
                AccountID = user.Account.ID,
                WorldID = gameStage.Server!.WorldID
            });
            var characters = (await _characterRepository
                    .RetrieveAllByAccountWorld(accountWorld.ID))
                .ToImmutableList();
            var packet = new PacketWriter();

            packet.WriteShort(6);

            packet.WriteByte((byte)result);

            if (result == LoginResult.Success)
            {
                user.State = LoginState.SelectCharacter;
                user.AccountWorld = accountWorld;
                user.SelectedWorldID = (byte)gameStage.Server!.WorldID;
                user.SelectedChannelID = (byte)gameStage.Server!.ChannelID;

                user.Account.LatestConnectedWorld = user.SelectedWorldID;

                packet.WriteString("normal");
                packet.WriteInt(accountWorld.Trunk.SlotMax);
                packet.WriteByte(0);
                packet.WriteInt(0);
                packet.WriteLong(0);
                packet.WriteBool(false);

                packet.WriteInt(0);
                packet.WriteByte(0);
                /*
                packet.WriteByte((byte)characters.Count);

                foreach (var character in characters)
                {
                    packet.WriteCharacterStats(character);
                    packet.WriteCharacterLooks(character);

                    packet.WriteBool(false);
                    packet.WriteBool(false);
                }
                */

                packet.WriteBool(!string.IsNullOrEmpty(user.Account.SPW));
                packet.WriteBool(false);
                packet.WriteInt(accountWorld.CharacterSlotMax);
                packet.WriteInt(0);
                packet.WriteInt(-1);
                packet.WriteLong(0);
                packet.WriteByte(0);
                packet.WriteByte(0);
            }

            await user.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter();

            packet.WriteShort(6);

            packet.WriteByte((byte)LoginResult.DBFail);

            await user.Dispatch(packet);
        }
    }
}
