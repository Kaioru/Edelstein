using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserMigrateToCashShopRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            try
            {
                var services = user.Service.Peers
                    .OfType<ShopServiceInfo>()
                    .Where(g => g.Worlds.Contains(user.Service.Info.WorldID))
                    .OrderBy(g => g.ID)
                    .ToList();
                var service = services.First();

                if (services.Count > 1)
                {
                    var id = await user.Prompt<int>(target => target.AskMenu(
                        "Which service should I connect to?", services.ToDictionary(
                            s => Convert.ToInt32(s.ID),
                            s => s.Name
                        ))
                    );
                    service = services.First(s => s.ID == id);
                }

                await user.Socket.TryMigrateTo(user.Account, user.Character, service);
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
                {
                    p.Encode<byte>(0x2);
                    await user.SendPacket(p);
                }
            }
        }
    }
}