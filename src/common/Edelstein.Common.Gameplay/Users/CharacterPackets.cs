using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Common.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Users
{
    public static class CharacterPackets
    {
        public static void WriteCharacterData(this IPacketWriter p, Character c, CharacterDataFlags flags = CharacterDataFlags.All)
        {
            p.WriteLong((long)flags);
            p.WriteByte(0);
            p.WriteByte(0);

            if (flags.HasFlag(CharacterDataFlags.Character))
            {
                p.WriteCharacterStats(c);

                p.WriteByte(250); // nFriendMax
                p.WriteBool(false);
            }

            if (flags.HasFlag(CharacterDataFlags.Money)) p.WriteInt(c.Money);

            if (flags.HasFlag(CharacterDataFlags.InventorySize))
            {
                if (flags.HasFlag(CharacterDataFlags.ItemSlotEquip))
                    p.WriteByte((byte)c.Inventories[ItemInventoryType.Equip].SlotMax);
                if (flags.HasFlag(CharacterDataFlags.ItemSlotConsume))
                    p.WriteByte((byte)c.Inventories[ItemInventoryType.Consume].SlotMax);
                if (flags.HasFlag(CharacterDataFlags.ItemSlotInstall))
                    p.WriteByte((byte)c.Inventories[ItemInventoryType.Install].SlotMax);
                if (flags.HasFlag(CharacterDataFlags.ItemSlotEtc))
                    p.WriteByte((byte)c.Inventories[ItemInventoryType.Etc].SlotMax);
                if (flags.HasFlag(CharacterDataFlags.ItemSlotCash))
                    p.WriteByte((byte)c.Inventories[ItemInventoryType.Cash].SlotMax);
            }

            if (flags.HasFlag(CharacterDataFlags.AdminShopCount))
            {
                p.WriteInt(0);
                p.WriteInt(0);
            }

            if (flags.HasFlag(CharacterDataFlags.ItemSlotEquip))
            {
                var inventory = c.Inventories[ItemInventoryType.Equip].Items;
                var equip = inventory.Where(kv => kv.Key >= 0);
                var equipped = inventory.Where(kv => kv.Key >= -100 && kv.Key < 0);
                var equipped2 = inventory.Where(kv => kv.Key >= -1000 && kv.Key < -100);
                var dragonEquipped = inventory.Where(kv => kv.Key >= -1100 && kv.Key < -1000);
                var mechanicEquipped = inventory.Where(kv => kv.Key >= -1200 && kv.Key < -1100);

                new List<IEnumerable<KeyValuePair<short, AbstractItemSlot>>>
                    {
                        equipped, equipped2, equip, dragonEquipped, mechanicEquipped
                    }
                    .ForEach(e =>
                    {
                        e.ForEach(kv =>
                        {
                            p.WriteShort((short)(Math.Abs(kv.Key) % 100));
                            p.WriteItemData(kv.Value);
                        });
                        p.WriteShort(0);
                    });
            }

            new List<(CharacterDataFlags, ItemInventoryType)>
                {
                    (CharacterDataFlags.ItemSlotConsume, ItemInventoryType.Consume),
                    (CharacterDataFlags.ItemSlotInstall, ItemInventoryType.Install),
                    (CharacterDataFlags.ItemSlotEtc, ItemInventoryType.Etc),
                    (CharacterDataFlags.ItemSlotCash, ItemInventoryType.Cash)
                }
                .Where(t => flags.HasFlag(t.Item1))
                .ForEach(t =>
                {
                    var inventory = c.Inventories[t.Item2].Items;

                    inventory.ForEach(kv =>
                    {
                        p.WriteByte((byte)kv.Key);
                        p.WriteItemData(kv.Value);
                    });
                    p.WriteByte(0);
                });

            if (flags.HasFlag(CharacterDataFlags.SkillRecord))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.SkillCooltime))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.QuestRecord))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.QuestComplete))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.MinigameRecord))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.CoupleRecord))
            {
                p.WriteShort(0); // Couple
                p.WriteShort(0); // Friend
                p.WriteShort(0); // Marriage
            }

            if (flags.HasFlag(CharacterDataFlags.MapTransfer))
            {
                for (var i = 0; i < 5; i++) p.WriteInt(0);
                for (var i = 0; i < 10; i++) p.WriteInt(0);
            }

            if (flags.HasFlag(CharacterDataFlags.NewYearCard))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.QuestRecordEx))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.WildHunterInfo))
            {
                if (c.Job / 100 == 33)
                {
                    p.WriteByte(0);
                    for (var i = 0; i < 5; i++) p.WriteInt(0);
                }
            }

            if (flags.HasFlag(CharacterDataFlags.QuestCompleteOld))
            {
                p.WriteShort(0);
            }

            if (flags.HasFlag(CharacterDataFlags.VisitorLog))
            {
                p.WriteShort(0);
            }
        }

        public static void WriteCharacterStats(this IPacketWriter p, Character c)
        {
            p.WriteInt(c.ID);
            p.WriteString(c.Name, 13);

            p.WriteByte(c.Gender);
            p.WriteByte(c.Skin);
            p.WriteInt(c.Face);
            p.WriteInt(c.Hair);

            c.Pets.ForEach(sn => p.WriteLong(sn));

            p.WriteByte(c.Level);
            p.WriteShort(c.Job);
            p.WriteShort(c.STR);
            p.WriteShort(c.DEX);
            p.WriteShort(c.INT);
            p.WriteShort(c.LUK);
            p.WriteInt(c.HP);
            p.WriteInt(c.MaxHP);
            p.WriteInt(c.MP);
            p.WriteInt(c.MaxMP);

            p.WriteShort(c.AP);
            if (c.Job / 1000 == 3 || c.Job / 100 == 22 || c.Job == 2001) // TODO: constants?
                c.WriteExtendSP(p);
            else
                p.WriteShort(c.SP);

            p.WriteInt(c.EXP);
            p.WriteShort(c.POP);

            p.WriteInt(c.TempEXP);
            p.WriteInt(c.FieldID);
            p.WriteByte(c.FieldPortal);
            p.WriteInt(c.PlayTime);
            p.WriteShort(c.SubJob);
        }

        public static void WriteCharacterLook(this IPacketWriter p, Character c)
        {
            p.WriteByte(c.Gender);
            p.WriteByte(c.Skin);
            p.WriteInt(c.Face);

            p.WriteBool(false);
            p.WriteInt(c.Hair);

            var inventory = c.Inventories[ItemInventoryType.Equip];
            var equipped = inventory.Items
                .Where(kv => kv.Key < 0)
                .ToDictionary();
            var stickers = equipped
                .Select(kv => equipped.ContainsKey((short)(kv.Key - 100))
                    ? new KeyValuePair<short, AbstractItemSlot>(kv.Key, equipped[(short)(kv.Key - 100)])
                    : kv)
                .ToDictionary();
            var unseen = equipped
                .Except(stickers)
                .ToDictionary(
                    kv => kv.Key <= -100 ? kv.Key - 100 : kv.Key,
                    kv => kv.Value
                );

            stickers.ForEach(kv =>
            {
                p.WriteByte((byte)Math.Abs(kv.Key));
                p.WriteInt(kv.Value.TemplateID);
            });
            p.WriteByte(0xFF);

            unseen.ForEach(kv =>
            {
                p.WriteByte((byte)Math.Abs(kv.Key));
                p.WriteInt(kv.Value.TemplateID);
            });
            p.WriteByte(0xFF);

            p.WriteInt(inventory.Items.ContainsKey(-111)
                ? inventory.Items[-111].TemplateID
                : 0);

            for (var i = 0; i < 3; i++)
                p.WriteInt(0);
        }

        private static void WriteExtendSP(this Character c, IPacketWriter p)
        {
            p.WriteByte((byte)c.ExtendSP.Count);
            c.ExtendSP.ForEach(kv =>
            {
                p.WriteByte(kv.Key);
                p.WriteByte(kv.Value);
            });
        }
    }
}
