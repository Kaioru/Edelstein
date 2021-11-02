using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Dialogs;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Dialogs
{
    public class ShopDialog : IShopDialog
    {
        public int TemplateID { get; }
        public IShopDialogInfo Info { get; }

        public ShopDialog(int templateID, IShopDialogInfo info)
        {
            TemplateID = templateID;
            Info = info;
        }

        public Task Enter(IFieldObjUser user)
        {
            var items = Info.Items.Values
                .OrderBy(i => i.ID)
                .ToList();
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.OpenShopDlg);

            packet.WriteInt(TemplateID);
            packet.WriteShort((short)items.Count);

            items.ForEach(i =>
            {
                packet.WriteInt(i.TemplateID);
                packet.WriteInt(i.Price);
                packet.WriteByte(i.DiscountRate);
                packet.WriteInt(i.TokenTemplateID);
                packet.WriteInt(i.TokenPrice);
                packet.WriteInt(i.ItemPeriod);
                packet.WriteInt(i.LevelLimited);

                if (GameConstants.IsRechargeableItem(i.TemplateID))
                    packet.WriteDouble(i.UnitPrice);
                else
                    packet.WriteShort((short)i.Quantity);

                packet.WriteShort(i.MaxPerSlot);
            });

            return user.Dispatch(packet);
        }

        public Task Leave(IFieldObjUser user)
            => user.EndDialog();
    }
}
