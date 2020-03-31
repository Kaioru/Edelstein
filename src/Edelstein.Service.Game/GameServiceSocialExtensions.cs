using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social;
using Edelstein.Core.Gameplay.Social.Memo;
using Edelstein.Core.Gameplay.Social.Memo.Events;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game
{
    public static class GameServiceSocialExtensions
    {
        public static async Task SubscribeSocialEvents(this GameService service, CancellationToken cancellationToken)
        {
            await service.Bus.SubscribeAsync<GroupMessageEvent>(
                async (msg, token) =>
                {
                    var users = service.FieldManager
                        .GetAll()
                        .SelectMany(f => f.GetObjects<IFieldUser>())
                        .Where(u => u.Character.Name != msg.Name)
                        .Where(u => msg.Recipients.Any(i => i == u.ID))
                        .ToList();

                    await Task.WhenAll(users
                        .Select(async u =>
                        {
                            using var p = new Packet(SendPacketOperations.GroupMessage);
                            p.Encode<byte>((byte) msg.Type);
                            p.Encode<string>(msg.Name);
                            p.Encode<string>(msg.Text);
                            await u.SendPacket(p);
                        })
                    );
                }, cancellationToken);
            await service.Bus.SubscribeAsync<MemoReceiveEvent>(
                async (msg, token) =>
                {
                    var user = service.FieldManager
                        .GetAll()
                        .SelectMany(f => f.GetObjects<IFieldUser>())
                        .FirstOrDefault(u => u.Character.ID == msg.CharacterID);
                    var memo = new SocialMemo(service.MemoManager, msg.Memo);

                    if (user == null) return;

                    user.Memos[memo.ID] = memo;

                    using var p = new Packet(SendPacketOperations.MemoResult);

                    p.Encode<byte>((byte) MemoResultType.Load);
                    p.Encode<byte>(1);
                    memo.EncodeData(p);

                    await user.SendPacket(p);
                }, cancellationToken);
        }
    }
}