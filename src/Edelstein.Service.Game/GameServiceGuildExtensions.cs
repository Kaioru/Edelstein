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
                        .Where(u => msg.Members.Any(m => m.CharacterID == u.ID))
                        .ToList();

                    users.ForEach(u => u.Guild = new SocialGuild(
                        u.Service.GuildManager,
                        msg.Guild,
                        msg.Members
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
        }
    }
}