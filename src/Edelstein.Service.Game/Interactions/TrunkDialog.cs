using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Core.Types;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Life.NPC;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq;

namespace Edelstein.Service.Game.Interactions
{
    public class TrunkDialog : AbstractDialog
    {
        private readonly NPCTemplate _template;
        private readonly ItemTrunk _trunk;

        public TrunkDialog(FieldUser user, NPCTemplate template, ItemTrunk trunk) : base(user)
        {
            _template = template;
            _trunk = trunk;
        }

        public override async Task Enter()
        {
            using (var p = new Packet(SendPacketOperations.TrunkResult))
            {
                p.Encode<byte>((byte) TrunkResult.OpenTrunkDlg);
                p.Encode<int>(_template.ID);
                EncodeItems(p);
                await User.SendPacket(p);
            }
        }

        public override async Task Leave()
        {
            await User.Interact(close: true);
        }

        public void EncodeItems(IPacket packet, DbChar flags = DbChar.All)
        {
            packet.Encode<byte>((byte) _trunk.SlotMax);
            packet.Encode<long>((long) flags);

            if (flags.HasFlag(DbChar.Money)) packet.Encode<int>(_trunk.Money);
            new Dictionary<DbChar, ItemInventoryType>
                {
                    [DbChar.ItemSlotEquip] = ItemInventoryType.Equip,
                    [DbChar.ItemSlotConsume] = ItemInventoryType.Consume,
                    [DbChar.ItemSlotInstall] = ItemInventoryType.Install,
                    [DbChar.ItemSlotEtc] = ItemInventoryType.Etc,
                    [DbChar.ItemSlotCash] = ItemInventoryType.Cash
                }
                .Where(kv => flags.HasFlag(kv.Key))
                .ForEach(kv =>
                {
                    var items = User.AccountData.Trunk.Items.Values
                        .Where(i => (ItemInventoryType) (i.TemplateID / 1000000) == kv.Value)
                        .ToList();

                    packet.Encode<byte>((byte) items.Count);
                    items.ForEach(i => i.Encode(packet));
                });
        }

        public async Task<TrunkResult> Get(ItemInventoryType type, short position)
        {
            var item = _trunk.Items.Values
                .Where(i => (ItemInventoryType) (i.TemplateID / 1000000) == type)
                .ToList()[position];

            if (!User.Character.HasSlotFor(item)) return TrunkResult.GetHavingOnlyItem;
            if (User.Character.Money < _template.TrunkGet) return TrunkResult.GetNoMoney;

            await User.ModifyStats(s => s.Money -= _template.TrunkGet);
            await User.ModifyInventory(i => i.Add(item));
            new ModifyInventoryContext(_trunk).Remove(item);
            return TrunkResult.GetSuccess;
        }

        public async Task<TrunkResult> Put(short position, int templateID, short count)
        {
            var inventory = User.Character.Inventories[(ItemInventoryType) (templateID / 1000000)];
            var item = inventory.Items[position];

            if (_trunk.Items.Count >= _trunk.SlotMax) return TrunkResult.PutNoSpace;
            if (User.Character.Money < _template.TrunkPut) return TrunkResult.PutNoMoney;

            await User.ModifyStats(s => s.Money -= _template.TrunkPut);
            await User.ModifyInventory(i =>
            {
                if (!ItemConstants.IsTreatSingly(item.TemplateID) &&
                    item is ItemSlotBundle bundle)
                {
                    if (bundle.Number < count) count = bundle.Number;
                    item = i.Take(bundle, count);
                }
                else i.Remove(item);
            });
            new ModifyInventoryContext(_trunk).Add(item);
            return TrunkResult.PutSuccess;
        }

        public async Task<TrunkResult> Sort()
        {
            return TrunkResult.SortItem;
        }

        public async Task<TrunkResult> Transact(int amount)
        {
            if (amount < 0 && User.Character.Money < amount ||
                int.MaxValue - User.Character.Money < amount)
                return TrunkResult.PutNoMoney;
            if (amount > 0 && _trunk.Money < amount ||
                int.MaxValue - _trunk.Money < amount)
                return TrunkResult.GetNoMoney;

            _trunk.Money += -amount;
            await User.ModifyStats(s => s.Money += amount);
            return TrunkResult.MoneySuccess;
        }
    }
}