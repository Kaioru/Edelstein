using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Entities.Characters;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using MoreLinq;

namespace Edelstein.Core.Gameplay.Extensions.Packets
{
    public static class CharacterPackets
    {
        public static void EncodeData(this Character c, IPacketEncoder p, DbChar flags = DbChar.All)
        {
            p.EncodeLong((long) flags);
            p.EncodeByte(0);
            p.EncodeByte(0);

            if (flags.HasFlag(DbChar.Character))
            {
                EncodeStats(c, p);
                p.EncodeByte(250); // nFriendMax
                p.EncodeBool(false);
            }

            if (flags.HasFlag(DbChar.Money)) p.EncodeInt(c.Money);

            if (flags.HasFlag(DbChar.InventorySize))
            {
                if (flags.HasFlag(DbChar.ItemSlotEquip))
                    p.EncodeByte((byte) c.Inventories[ItemInventoryType.Equip].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotConsume))
                    p.EncodeByte((byte) c.Inventories[ItemInventoryType.Consume].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotInstall))
                    p.EncodeByte((byte) c.Inventories[ItemInventoryType.Install].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotEtc))
                    p.EncodeByte((byte) c.Inventories[ItemInventoryType.Etc].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotCash))
                    p.EncodeByte((byte) c.Inventories[ItemInventoryType.Cash].SlotMax);
            }

            if (flags.HasFlag(DbChar.AdminShopCount))
            {
                p.EncodeInt(0);
                p.EncodeInt(0);
            }

            if (flags.HasFlag(DbChar.ItemSlotEquip))
            {
                var inventory = c.Inventories[ItemInventoryType.Equip].Items;
                var equip = inventory.Where(kv => kv.Key >= 0);
                var equipped = inventory.Where(kv => kv.Key >= -100 && kv.Key < 0);
                var equipped2 = inventory.Where(kv => kv.Key >= -1000 && kv.Key < -100);
                var dragonEquipped = inventory.Where(kv => kv.Key >= -1100 && kv.Key < -1000);
                var mechanicEquipped = inventory.Where(kv => kv.Key >= -1200 && kv.Key < -1100);

                new List<IEnumerable<KeyValuePair<short, ItemSlot>>>
                    {
                        equipped, equipped2, equip, dragonEquipped, mechanicEquipped
                    }
                    .ForEach(e =>
                    {
                        e.ForEach(kv =>
                        {
                            p.EncodeShort((short) (Math.Abs(kv.Key) % 100));
                            kv.Value.Encode(p);
                        });
                        p.EncodeShort(0);
                    });
            }

            new List<(DbChar, ItemInventoryType)>
                {
                    (DbChar.ItemSlotConsume, ItemInventoryType.Consume),
                    (DbChar.ItemSlotInstall, ItemInventoryType.Install),
                    (DbChar.ItemSlotEtc, ItemInventoryType.Etc),
                    (DbChar.ItemSlotCash, ItemInventoryType.Cash)
                }
                .Where(t => flags.HasFlag(t.Item1))
                .ForEach(t =>
                {
                    var inventory = c.Inventories[t.Item2].Items;

                    inventory.ForEach(kv =>
                    {
                        p.EncodeByte((byte) kv.Key);
                        kv.Value.Encode(p);
                    });
                    p.EncodeByte(0);
                });

            if (flags.HasFlag(DbChar.SkillRecord))
            {
                p.EncodeShort((short) c.SkillRecord.Count);
                c.SkillRecord.ForEach(kv =>
                {
                    p.EncodeInt(kv.Key);
                    p.EncodeInt(kv.Value.Level);
                    p.EncodeDateTime(kv.Value.DateExpire ?? ItemConstants.Permanent);

                    if (SkillConstants.IsSkillNeedMasterLevel(kv.Key))
                        p.EncodeInt(kv.Value.MasterLevel);
                });
            }

            if (flags.HasFlag(DbChar.SkillCooltime))
            {
                p.EncodeShort(0);
            }

            if (flags.HasFlag(DbChar.QuestRecord))
            {
                p.EncodeShort((short) c.QuestRecord.Count);
                c.QuestRecord.ForEach(q =>
                {
                    p.EncodeShort(q.Key);
                    p.EncodeString(q.Value);
                });
            }

            if (flags.HasFlag(DbChar.QuestComplete))
            {
                p.EncodeShort((short) c.QuestComplete.Count);
                c.QuestComplete.ForEach(q =>
                {
                    p.EncodeShort(q.Key);
                    p.EncodeDateTime(q.Value);
                });
            }

            if (flags.HasFlag(DbChar.MinigameRecord))
            {
                p.EncodeShort(0);
            }

            if (flags.HasFlag(DbChar.CoupleRecord))
            {
                p.EncodeShort(0); // Couple
                p.EncodeShort(0); // Friend
                p.EncodeShort(0); // Marriage
            }

            if (flags.HasFlag(DbChar.MapTransfer))
            {
                for (var i = 0; i < 5; i++) p.EncodeInt(0);
                for (var i = 0; i < 10; i++) p.EncodeInt(0);
            }

            if (flags.HasFlag(DbChar.NewYearCard))
            {
                p.EncodeShort(0);
            }

            if (flags.HasFlag(DbChar.QuestRecordEx))
            {
                p.EncodeShort((short) c.QuestRecordEx.Count);
                c.QuestRecordEx.ForEach(q =>
                {
                    p.EncodeShort(q.Key);
                    p.EncodeString(q.Value);
                });
            }

            if (flags.HasFlag(DbChar.WildHunterInfo))
            {
                if (c.Job / 100 == 33)
                {
                    p.EncodeByte(0);
                    for (var i = 0; i < 5; i++) p.EncodeInt(0);
                }
            }

            if (flags.HasFlag(DbChar.QuestCompleteOld))
            {
                p.EncodeShort(0);
            }

            if (flags.HasFlag(DbChar.VisitorLog))
            {
                p.EncodeShort(0);
            }
        }

        public static void EncodeStats(this Character c, IPacketEncoder p)
        {
            p.EncodeInt(c.ID);
            p.EncodeString(c.Name, 13);

            p.EncodeByte(c.Gender);
            p.EncodeByte(c.Skin);
            p.EncodeInt(c.Face);
            p.EncodeInt(c.Hair);

            c.Pets.ForEach(sn => p.EncodeLong(sn));

            p.EncodeByte(c.Level);
            p.EncodeShort(c.Job);
            p.EncodeShort(c.STR);
            p.EncodeShort(c.DEX);
            p.EncodeShort(c.INT);
            p.EncodeShort(c.LUK);
            p.EncodeInt(c.HP);
            p.EncodeInt(c.MaxHP);
            p.EncodeInt(c.MP);
            p.EncodeInt(c.MaxMP);

            p.EncodeShort(c.AP);
            if (SkillConstants.IsExtendSPJob(c.Job))
                c.EncodeExtendSP(p);
            else
                p.EncodeShort(c.SP);

            p.EncodeInt(c.EXP);
            p.EncodeShort(c.POP);

            p.EncodeInt(c.TempEXP);
            p.EncodeInt(c.FieldID);
            p.EncodeByte(c.FieldPortal);
            p.EncodeInt(c.PlayTime);
            p.EncodeShort(c.SubJob);
        }

        public static void EncodeLook(this Character c, IPacketEncoder p)
        {
            p.EncodeByte(c.Gender);
            p.EncodeByte(c.Skin);
            p.EncodeInt(c.Face);

            p.EncodeBool(false);
            p.EncodeInt(c.Hair);

            var inventory = c.Inventories[ItemInventoryType.Equip];
            var equipped = inventory.Items
                .Where(kv => kv.Key < 0)
                .ToDictionary();
            var stickers = equipped
                .Select(kv => equipped.ContainsKey((short) (kv.Key - 100))
                    ? new KeyValuePair<short, ItemSlot>(kv.Key, equipped[(short) (kv.Key - 100)])
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
                p.EncodeByte((byte) Math.Abs(kv.Key));
                p.EncodeInt(kv.Value.TemplateID);
            });
            p.EncodeByte(0xFF);

            unseen.ForEach(kv =>
            {
                p.EncodeByte((byte) Math.Abs(kv.Key));
                p.EncodeInt(kv.Value.TemplateID);
            });
            p.EncodeByte(0xFF);

            p.EncodeInt(inventory.Items.ContainsKey(-111)
                ? inventory.Items[-111].TemplateID
                : 0);

            for (var i = 0; i < 3; i++)
                p.EncodeInt(0);
        }

        public static void EncodeExtendSP(this Character c, IPacketEncoder p)
        {
            p.EncodeByte((byte) c.ExtendSP.Count);
            c.ExtendSP.ForEach(kv =>
            {
                p.EncodeByte(kv.Key);
                p.EncodeByte(kv.Value);
            });
        }
    }
}