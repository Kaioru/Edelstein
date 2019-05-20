using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Interactions;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserShopRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var request = (ShopRequest) packet.Decode<byte>();

            if (!(user.Dialog is ShopDialog shop)) return;
            if (request == ShopRequest.Close)
            {
                await user.Interact(close: true);
                return;
            }

            using (var p = new Packet(SendPacketOperations.ShopResult))
            {
                switch (request)
                {
                    case ShopRequest.Buy:
                    {
                        var position = packet.Decode<short>();
                        var templateID = packet.Decode<int>();
                        var count = packet.Decode<short>();
                        p.Encode<byte>((byte) await shop.Buy(position, count));
                        break;
                    }

                    case ShopRequest.Sell:
                    {
                        var position = packet.Decode<short>();
                        var templateID = packet.Decode<int>();
                        var count = packet.Decode<short>();
                        p.Encode<byte>((byte) await shop.Sell(position, templateID, count));
                        break;
                    }

                    case ShopRequest.Recharge:
                    {
                        var position = packet.Decode<short>();
                        p.Encode<byte>((byte) await shop.Recharge(position));
                        break;
                    }
                }

                await user.SendPacket(p);
            }
        }
    }
}