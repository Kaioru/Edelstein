using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Types;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Interactions;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserTrunkRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var request = (TrunkRequest) packet.Decode<byte>();

            if (!(user.Dialog is TrunkDialog trunk)) return;
            if (request == TrunkRequest.CloseDialog)
            {
                await user.Interact(close: true);
                return;
            }

            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                switch (request)
                {
                    case TrunkRequest.GetItem:
                    {
                        var type = (ItemInventoryType) packet.Decode<byte>();
                        var position = packet.Decode<byte>();
                        p.Encode<byte>((byte) await trunk.Get(type, position));
                        trunk.EncodeItems(p);
                        break;
                    }

                    case TrunkRequest.PutItem:
                    {
                        var position = packet.Decode<short>();
                        var templateID = packet.Decode<int>();
                        var count = packet.Decode<short>();
                        p.Encode<byte>((byte) await trunk.Put(position, templateID, count));
                        trunk.EncodeItems(p);
                        break;
                    }

                    case TrunkRequest.SortItem:
                    {
                        p.Encode<byte>((byte) await trunk.Sort());
                        trunk.EncodeItems(p);
                        break;
                    }

                    case TrunkRequest.Money:
                    {
                        var amount = packet.Decode<int>();
                        p.Encode<byte>((byte) await trunk.Transact(amount));
                        trunk.EncodeItems(p, DbChar.Money);
                        break;
                    }
                }

                await user.SendPacket(p);
            }
        }
    }
}