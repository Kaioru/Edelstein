using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Guild.Events;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game
{
    public static class GameServiceGuildExtensions
    {
        private static ICollection<IFieldUser> GetGuildMembers(this GameService service, IGuildEvent msg)
        {
            return service.FieldManager
                .GetAll()
                .SelectMany(f => f.GetObjects<IFieldUser>())
                .Where(u => u.Guild != null)
                .Where(u => u.Guild.ID == msg.GuildID)
                .ToList();
        }

        private static IFieldUser? GetGuildMember(this GameService service, IGuildMemberEvent msg)
        {
            return service
                .GetGuildMembers(msg)
                .FirstOrDefault(m => m.Character.ID == msg.GuildMemberID);
        }

        public static async Task SubscribeGuildEvents(this GameService service, CancellationToken cancellationToken)
        {
            await service.Bus.SubscribeAsync<GuildCreateEvent>(
                async (msg, token) =>
                {
                    var users = service.FieldManager
                        .GetAll()
                        .SelectMany(f => f.GetObjects<IFieldUser>())
                        .Where(u => msg.GuildMembers.Any(m => m.CharacterID == u.ID))
                        .ToList();

                    users.ForEach(u => u.Guild = new SocialGuild(
                        u.Service.GuildManager,
                        msg.Guild,
                        msg.GuildMembers
                    ));
                    await Task.WhenAll(users.Select(async u =>
                    {
                        using var p1 = new Packet(SendPacketOperations.GuildResult);

                        p1.Encode<byte>((byte) GuildResultType.CreateNewGuild_Done);
                        u.Guild.EncodeData(p1);

                        await u.SendPacket(p1);

                        using var p2 = new Packet(SendPacketOperations.UserGuildNameChanged);

                        p2.Encode<int>(u.ID);
                        p2.Encode<string>(u.Guild.Name);

                        await u.Field.BroadcastPacket(u, p2);
                    }));
                }, cancellationToken);
            await service.Bus.SubscribeAsync<GuildJoinEvent>(
                async (msg, token) =>
                {
                    var user = service.FieldManager
                        .GetAll()
                        .SelectMany(f => f.GetObjects<IFieldUser>())
                        .FirstOrDefault(u => u.ID == msg.GuildMemberID);

                    if (user != null)
                    {
                        user.Guild = new SocialGuild(
                            service.GuildManager,
                            msg.Guild,
                            msg.GuildMembers
                        );
                    }

                    var users = service.GetGuildMembers(msg);
                    var record = msg.GuildMembers
                        .FirstOrDefault(m => m.CharacterID == msg.GuildMemberID);

                    if (record == null) return;

                    await Task.WhenAll(users
                        .Select(async u =>
                        {
                            var member = new SocialGuildMember(
                                u.Service.GuildManager,
                                u.Guild,
                                record
                            );

                            if (u != user) await u.Guild.OnUpdateJoin(member);

                            using var p = new Packet(SendPacketOperations.GuildResult);

                            p.Encode<byte>((byte) GuildResultType.JoinGuild_Done);
                            p.Encode<int>(msg.GuildID);
                            p.Encode<int>(msg.GuildMemberID);

                            if (u.ID != msg.GuildMemberID)
                                member.EncodeData(p);

                            await u.SendPacket(p);
                        }));
                    if (user != null)
                    {
                        using var p = new Packet(SendPacketOperations.UserGuildNameChanged);

                        p.Encode<int>(user.ID);
                        p.Encode<string>(user.Guild.Name);

                        await user.Field.BroadcastPacket(user, p);
                    }
                }, cancellationToken);
        }
    }
}