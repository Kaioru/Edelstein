using System;
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
            await service.Bus.SubscribeAsync<GuildNotifyLoginOrLogoutEvent>(async (msg, token) =>
            {
                var users = service.GetGuildMembers(msg);
                var user = service.GetGuildMember(msg);

                await Task.WhenAll(users
                    .Except(new[] {user})
                    .Select(async u =>
                    {
                        await u.Guild.OnUpdateNotifyLoginOrLogout(
                            msg.GuildMemberID,
                            msg.Online
                        );

                        using var p = new Packet(SendPacketOperations.GuildResult);

                        p.Encode<byte>((byte) GuildResultType.NotifyLoginOrLogout);
                        p.Encode<int>(msg.GuildID);
                        p.Encode<int>(msg.GuildMemberID);
                        p.Encode<bool>(msg.Online);

                        await u.SendPacket(p);
                    }));
            }, cancellationToken);
            await service.Bus.SubscribeAsync<GuildChangeLevelOrJobEvent>(async (msg, token) =>
            {
                var users = service.GetGuildMembers(msg);

                await Task.WhenAll(users.Select(u => u.Guild.OnUpdateChangeLevelOrJob(
                    msg.GuildMemberID,
                    msg.Level,
                    msg.Job
                )));
                await Task.WhenAll(users.Select(async u =>
                {
                    using var p = new Packet(SendPacketOperations.GuildResult);

                    p.Encode<byte>((byte) GuildResultType.ChangeLevelOrJob);
                    p.Encode<int>(msg.GuildID);
                    p.Encode<int>(msg.GuildMemberID);
                    p.Encode<int>(msg.Level);
                    p.Encode<int>(msg.Job);

                    await u.SendPacket(p);
                }));
            }, cancellationToken);
            await service.Bus.SubscribeAsync<GuildSetGradeNameEvent>(async (msg, token) =>
            {
                var users = service.GetGuildMembers(msg);

                await Task.WhenAll(users.Select(u => u.Guild.OnUpdateSetGradeName(
                    msg.GradeName
                )));
                await Task.WhenAll(users.Select(async u =>
                {
                    using var p = new Packet(SendPacketOperations.GuildResult);

                    p.Encode<byte>((byte) GuildResultType.SetGradeName_Done);
                    p.Encode<int>(msg.GuildID);

                    foreach (var name in msg.GradeName)
                        p.Encode<string>(name);

                    await u.SendPacket(p);
                }));
            }, cancellationToken);
            await service.Bus.SubscribeAsync<GuildSetMemberGradeEvent>(async (msg, token) =>
            {
                var users = service.GetGuildMembers(msg);

                await Task.WhenAll(users.Select(u => u.Guild.OnUpdateSetMemberGrade(
                    msg.GuildMemberID,
                    msg.Grade
                )));
                await Task.WhenAll(users.Select(async u =>
                {
                    using var p = new Packet(SendPacketOperations.GuildResult);

                    p.Encode<byte>((byte) GuildResultType.SetMemberGrade_Done);
                    p.Encode<int>(msg.GuildID);
                    p.Encode<int>(msg.GuildMemberID);
                    p.Encode<byte>(msg.Grade);

                    await u.SendPacket(p);
                }));
            }, cancellationToken);
            await service.Bus.SubscribeAsync<GuildSetMarkEvent>(async (msg, token) =>
            {
                var users = service.GetGuildMembers(msg);

                await Task.WhenAll(users.Select(u => u.Guild.OnUpdateSetMark(
                    msg.MarkBg,
                    msg.MarkBgColor,
                    msg.Mark,
                    msg.MarkColor
                )));
                await Task.WhenAll(users.Select(async u =>
                {
                    using var p1 = new Packet(SendPacketOperations.GuildResult);

                    p1.Encode<byte>((byte) GuildResultType.SetMark_Done);
                    p1.Encode<int>(msg.GuildID);
                    p1.Encode<short>(msg.MarkBg);
                    p1.Encode<byte>(msg.MarkBgColor);
                    p1.Encode<short>(msg.Mark);
                    p1.Encode<byte>(msg.MarkColor);

                    await u.SendPacket(p1);

                    using var p2 = new Packet(SendPacketOperations.UserGuildMarkChanged);

                    p2.Encode<int>(u.ID);
                    p2.Encode<short>(msg.MarkBg);
                    p2.Encode<byte>(msg.MarkBgColor);
                    p2.Encode<short>(msg.Mark);
                    p2.Encode<byte>(msg.MarkColor);

                    await u.Field.BroadcastPacket(u, p2);
                }));
            }, cancellationToken);
            await service.Bus.SubscribeAsync<GuildSetNoticeEvent>(async (msg, token) =>
            {
                var users = service.GetGuildMembers(msg);

                await Task.WhenAll(users.Select(u => u.Guild.OnUpdateSetNotice(
                    msg.Notice
                )));
                await Task.WhenAll(users.Select(async u =>
                {
                    using var p = new Packet(SendPacketOperations.GuildResult);

                    p.Encode<byte>((byte) GuildResultType.SetNotice_Done);
                    p.Encode<int>(msg.GuildID);
                    p.Encode<string>(msg.Notice);

                    await u.SendPacket(p);
                }));
            }, cancellationToken);
        }
    }
}