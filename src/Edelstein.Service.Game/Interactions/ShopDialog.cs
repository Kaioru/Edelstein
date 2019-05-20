using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Etc.NPCShop;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Interactions
{
    public class ShopDialog : AbstractDialog
    {
        private readonly NPCShopTemplate _template;

        public ShopDialog(FieldUser user, NPCShopTemplate template) : base(user)
        {
            _template = template;
        }

        public override async Task Enter()
        {
            using (var p = new Packet(SendPacketOperations.OpenShopDlg))
            {
                p.Encode<int>(_template.ID);

                var items = _template.Items.Values
                    .OrderBy(i => i.ID)
                    .ToList();

                p.Encode<short>((short) items.Count);
                items.ForEach(i =>
                {
                    p.Encode<int>(i.TemplateID);
                    p.Encode<int>(i.Price);
                    p.Encode<byte>(i.DiscountRate);
                    p.Encode<int>(i.TokenTemplateID);
                    p.Encode<int>(i.TokenPrice);
                    p.Encode<int>(i.ItemPeriod);
                    p.Encode<int>(i.LevelLimited);

                    if (!ItemConstants.IsRechargeableItem(i.TemplateID)) p.Encode<short>(i.Quantity);
                    else p.Encode<double>(i.UnitPrice);

                    p.Encode<short>(i.MaxPerSlot);
                });

                await User.SendPacket(p);
            }
        }

        public override Task Leave()
        {
            return Task.CompletedTask;
        }
    }
}