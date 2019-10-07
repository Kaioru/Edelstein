using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Extensions.Templates;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Etc.NPCShop;
using Edelstein.Provider.Templates.Item;
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
            using var p = new Packet(SendPacketOperations.OpenShopDlg);
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

        public override async Task Leave()
        {
            await User.Interact(close: true);
        }

        public async Task<ShopResult> Buy(short position, short count)
        {
            try
            {
                var shopItems = _template.Items
                    .OrderBy(kv => kv.Key)
                    .Select(kv => kv.Value)
                    .Where(i => i.Price > 0 ||
                                i.TokenPrice > 0)
                    .ToList();

                if (position > shopItems.Count)
                    return ShopResult.CantBuyAnymore;

                var shopItem = shopItems[position];
                var item = User.Service.TemplateManager.Get<ItemTemplate>(shopItem.TemplateID);

                if (shopItem.Quantity > 1) count = 1;

                count = Math.Max(count, shopItem.MaxPerSlot);
                count = Math.Min(count, (short) 1);

                if (item == null) return ShopResult.BuyUnknown;
                if (shopItem.Price > 0 &&
                    User.Character.Money < shopItem.Price * count)
                    return ShopResult.BuyNoMoney;
                if (shopItem.TokenTemplateID > 0 &&
                    User.Character.GetItemCount(shopItem.TokenTemplateID) > shopItem.TokenPrice * count)
                    return ShopResult.BuyNoToken;
                if (User.Character.Level < shopItem.LevelLimited)
                    return ShopResult.LimitLevel_Less;

                var itemSlot = item.ToItemSlot();

                if (itemSlot is ItemSlotBundle bundle)
                    if (ItemConstants.IsRechargeableItem(bundle.TemplateID))
                        bundle.Number = bundle.MaxNumber;
                    else
                        bundle.Number = (short) (count * shopItem.Quantity);
                if (!User.Character.HasSlotFor(itemSlot))
                    return ShopResult.BuyUnknown;

                if (shopItem.ItemPeriod > 0)
                    itemSlot.DateExpire = DateTime.Now.AddMinutes(shopItem.ItemPeriod);

                if (shopItem.Price > 0)
                    await User.ModifyStats(s => s.Money -= shopItem.Price * count);
                if (shopItem.TokenTemplateID > 0)
                    await User.ModifyInventory(i => i.Remove(
                        shopItem.TokenTemplateID,
                        (short) (shopItem.TokenPrice * count)
                    ));
                await User.ModifyInventory(i => i.Add(itemSlot));
                return ShopResult.BuySuccess;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ShopResult.BuySuccess;
            }
        }

        public async Task<ShopResult> Sell(short position, int templateID, short count)
        {
            var inventory = User.Character.Inventories[(ItemInventoryType) (templateID / 1000000)];

            if (!inventory.Items.ContainsKey(position))
                return ShopResult.SellUnknown;

            var itemSlot = inventory.Items[position];
            var item = User.Service.TemplateManager.Get<ItemTemplate>(itemSlot.TemplateID);
            var price = item.SellPrice;

            if (ItemConstants.IsRechargeableItem(item.ID))
                price += (int) ((itemSlot as ItemSlotBundle)?.Number *
                                (item as ItemBundleTemplate)?.UnitPrice ?? 0);
            else price *= count;

            if (int.MaxValue - User.Character.Money < price)
                return ShopResult.SellUnknown;

            await User.ModifyInventory(i => i.Remove(itemSlot, count));
            await User.ModifyStats(s => s.Money += price);
            return ShopResult.SellSuccess;
        }

        public Task<ShopResult> Recharge(short position)
        {
            // TODO
            return Task.FromResult(ShopResult.RechargeSuccess);
        }
    }
}