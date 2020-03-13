using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Gameplay.Social.Party.Events;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game
{
    public static class GameServiceExtensions
    {
        private static ICollection<IFieldUser> GetPartyMembers(this GameService service, IPartyEvent msg)
        {
            return service.FieldManager
                .GetAll()
                .SelectMany(f => f.GetObjects<IFieldUser>())
                .Where(u => u.Party != null)
                .Where(u => u.Party.ID == msg.PartyID)
                .ToList();
        }

        private static IFieldUser? GetPartyMember(this GameService service, IPartyMemberEvent msg)
        {
            return service
                .GetPartyMembers(msg)
                .FirstOrDefault(m => m.Character.ID == msg.PartyMemberID);
        }

        public static async Task SubscribePartyEvents(this GameService service, CancellationToken cancellationToken)
        {
            await service.Bus.SubscribeAsync<PartyJoinEvent>(
                async (msg, token) =>
                {
                    var user = service.FieldManager
                        .GetAll()
                        .SelectMany(f => f.GetObjects<IFieldUser>())
                        .FirstOrDefault(u => u.ID == msg.PartyMemberID);

                    if (user != null)
                    {
                        user.Party = new SocialParty(
                            service.PartyManager,
                            msg.Party,
                            msg.PartyMembers
                        );
                    }

                    var users = service.GetPartyMembers(msg);
                    var member = msg.PartyMembers
                        .FirstOrDefault(m => m.CharacterID == msg.PartyMemberID);

                    if (member == null) return;

                    await Task.WhenAll(users
                        .Except(new[] {user})
                        .Select(u => u.Party.OnUpdateJoin(new SocialPartyMember(
                            u.Service.PartyManager,
                            u.Party,
                            member
                        ))));
                    await Task.WhenAll(users.Select(async u =>
                    {
                        using var p = new Packet(SendPacketOperations.PartyResult);

                        p.Encode<byte>((byte) PartyResultType.JoinParty_Done);
                        p.Encode<int>(msg.PartyID);
                        p.Encode<string>(member.CharacterName);
                        u.Party.EncodeData(u.Service.State.ChannelID, p);

                        await u.SendPacket(p);
                    }));

                    if (user != null)
                    {
                        using var p = new Packet(SendPacketOperations.UserHP);

                        p.Encode<int>(user.ID);
                        p.Encode<int>(user.Character.HP);
                        p.Encode<int>(user.Character.MaxHP);

                        await user.Field.BroadcastPacket(user, user.Party, p);

                        await Task.WhenAll(user
                            .GetWatchedObjects<IFieldUser>()
                            .Where(u => u.Party?.ID == user.Party.ID)
                            .Where(u => u != user)
                            .Select(async u =>
                            {
                                using var p = new Packet(SendPacketOperations.UserHP);

                                p.Encode<int>(u.ID);
                                p.Encode<int>(u.Character.HP);
                                p.Encode<int>(u.Character.MaxHP);

                                await user.SendPacket(p);
                            }));
                    }
                }, cancellationToken);
            await service.Bus.SubscribeAsync<PartyWithdrawEvent>(
                async (msg, token) =>
                {
                    var user = service.GetPartyMember(msg);
                    var users = service.GetPartyMembers(msg);

                    await Task.WhenAll(users.Select(u => u.Party.OnUpdateWithdraw(msg.PartyMemberID)));
                    await Task.WhenAll(users.Select(async u =>
                    {
                        using var p = new Packet(SendPacketOperations.PartyResult);

                        p.Encode<byte>((byte) PartyResultType.WithdrawParty_Done);
                        p.Encode<int>(msg.PartyID);
                        p.Encode<int>(msg.PartyMemberID);
                        p.Encode<bool>(!msg.Disband);

                        if (!msg.Disband)
                        {
                            p.Encode<bool>(msg.Kick);
                            p.Encode<string>(msg.CharacterName);
                            u.Party.EncodeData(u.Service.State.ChannelID, p);
                        }

                        await u.SendPacket(p);
                    }));

                    if (user != null)
                        user.Party = null;
                }, cancellationToken);
            await service.Bus.SubscribeAsync<PartyChangeBossEvent>(
                async (msg, token) =>
                {
                    var users = service.GetPartyMembers(msg);

                    await Task.WhenAll(users.Select(u => u.Party.OnUpdateBoss(msg.PartyMemberID)));
                    await Task.WhenAll(users.Select(async u =>
                    {
                        using var p = new Packet(SendPacketOperations.PartyResult);

                        p.Encode<byte>((byte) PartyResultType.ChangePartyBoss_Done);
                        p.Encode<int>(msg.PartyMemberID);
                        p.Encode<bool>(msg.Disconnect);

                        await u.SendPacket(p);
                    }));
                }, cancellationToken);
            await service.Bus.SubscribeAsync<PartyUserMigrationEvent>(
                async (msg, token) =>
                {
                    var users = service.GetPartyMembers(msg);

                    await Task.WhenAll(users.Select(u => u.Party.OnUpdateUserMigration(
                        msg.PartyMemberID,
                        msg.ChannelID,
                        msg.FieldID
                    )));
                    await Task.WhenAll(users.Select(async u =>
                    {
                        using var p = new Packet(SendPacketOperations.PartyResult);

                        p.Encode<byte>((byte) PartyResultType.UserMigration);
                        p.Encode<int>(msg.PartyID);
                        u.Party.EncodeData(service.State.ChannelID, p);

                        await u.SendPacket(p);
                    }));
                }, cancellationToken);
            await service.Bus.SubscribeAsync<PartyChangeLevelOrJobEvent>(async (msg, token) =>
            {
                var users = service.GetPartyMembers(msg);

                await Task.WhenAll(users.Select(u => u.Party.OnUpdateChangeLevelOrJob(
                    msg.PartyMemberID,
                    msg.Level,
                    msg.Job
                )));
                await Task.WhenAll(users.Select(async u =>
                {
                    using var p = new Packet(SendPacketOperations.PartyResult);

                    p.Encode<byte>((byte) PartyResultType.ChangeLevelOrJob);
                    p.Encode<int>(msg.PartyMemberID);
                    p.Encode<int>(msg.Level);
                    p.Encode<int>(msg.Job);

                    await u.SendPacket(p);
                }));
            }, cancellationToken);
        }
    }
}