using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class NotifyPartyMemberUpdateLevelOrJobPlug : IPipelinePlug<NotifyPartyMemberUpdateLevelOrJob>
{
    private readonly IGameStage _stage;

    public NotifyPartyMemberUpdateLevelOrJobPlug(IGameStage stage) => _stage = stage;

    public async Task Handle(IPipelineContext ctx, NotifyPartyMemberUpdateLevelOrJob message)
    {
        var users = await _stage.Users.RetrieveAll();
        var partied = users
            .Where(u => u.Party?.PartyID == message.PartyID)
            .ToImmutableArray();

        foreach (var user in partied)
        {
            if (user.Party == null) continue;
            if (user.Party.CharacterID == message.CharacterID)
            {
                user.Party.Level = message.Level;
                user.Party.Job = message.Job;
            }

            if (user.Party.Members.TryGetValue(message.CharacterID, out var member))
            {
                member.Level = message.Level;
                member.Job = message.Job;
            }

            using var packet = new PacketWriter(PacketSendOperations.PartyResult);
            packet.WriteByte((byte)PartyResultOperations.ChangeLevelOrJob);
            packet.WriteInt(message.CharacterID);
            packet.WriteInt(message.Level);
            packet.WriteInt(message.Job);
            _ = user.Dispatch(packet.Build());
        }
    }
}
