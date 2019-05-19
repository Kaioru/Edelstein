using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Types;
using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using MoreLinq;

namespace Edelstein.Core.Extensions
{
    public static class CharacterPackets
    {
        public static void EncodeData(this Character c, IPacket p, DbChar flags = DbChar.All)
        {
            p.Encode<long>((long) flags);
            p.Encode<byte>(0);
            p.Encode<byte>(0);

            if (flags.HasFlag(DbChar.Character))
            {
                EncodeStats(c, p);
                p.Encode<byte>(250); // nFriendMax
                p.Encode<bool>(false);
            }

            if (flags.HasFlag(DbChar.Money)) p.Encode<int>(c.Money);

            if (flags.HasFlag(DbChar.InventorySize))
            {
                if (flags.HasFlag(DbChar.ItemSlotEquip))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Equip].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotConsume))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Consume].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotInstall))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Install].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotEtc))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Etc].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotCash))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Cash].SlotMax);
            }

            if (flags.HasFlag(DbChar.AdminShopCount))
            {
                p.Encode<int>(0);
                p.Encode<int>(0);
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
                            p.Encode<short>((short) (Math.Abs(kv.Key) % 100));
                            kv.Value.Encode(p);
                        });
                        p.Encode<short>(0);
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
                        p.Encode<byte>((byte) kv.Key);
                        kv.Value.Encode(p);
                    });
                    p.Encode<byte>(0);
                });

            if (flags.HasFlag(DbChar.SkillRecord))
            {
                p.Encode<short>((short) c.SkillRecord.Count);
                c.SkillRecord.ForEach(kv =>
                {
                    p.Encode<int>(kv.Key);
                    p.Encode<int>(kv.Value.Level);
                    p.Encode<DateTime>(kv.Value.DateExpire ?? ItemConstants.Permanent);

                    if (SkillConstants.IsSkillNeedMasterLevel(kv.Key))
                        p.Encode<int>(kv.Value.MasterLevel);
                });
            }

            if (flags.HasFlag(DbChar.SkillCooltime))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.QuestRecord))
            {
                p.Encode<short>((short) c.QuestRecord.Count);
                c.QuestRecord.ForEach(q =>
                {
                    p.Encode<short>(q.Key);
                    p.Encode<string>(q.Value);
                });
            }

            if (flags.HasFlag(DbChar.QuestComplete))
            {
                p.Encode<short>((short) c.QuestComplete.Count);
                c.QuestComplete.ForEach(q =>
                {
                    p.Encode<short>(q.Key);
                    p.Encode<DateTime>(q.Value);
                });
            }

            if (flags.HasFlag(DbChar.MinigameRecord))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.CoupleRecord))
            {
                p.Encode<short>(0); // Couple
                p.Encode<short>(0); // Friend
                p.Encode<short>(0); // Marriage
            }

            if (flags.HasFlag(DbChar.MapTransfer))
            {
                for (var i = 0; i < 5; i++) p.Encode<int>(0);
                for (var i = 0; i < 10; i++) p.Encode<int>(0);
            }

            if (flags.HasFlag(DbChar.NewYearCard))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.QuestRecordEx))
            {
                p.Encode<short>((short) c.QuestRecordEx.Count);
                c.QuestRecordEx.ForEach(q =>
                {
                    p.Encode<short>(q.Key);
                    p.Encode<string>(q.Value);
                });
            }

            if (flags.HasFlag(DbChar.WildHunterInfo))
            {
                if (c.Job / 100 == 33)
                {
                    p.Encode<byte>(0);
                    for (var i = 0; i < 5; i++) p.Encode<int>(0);
                }
            }

            if (flags.HasFlag(DbChar.QuestCompleteOld))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.VisitorLog))
            {
                p.Encode<short>(0);
            }
        }

        public static void EncodeStats(this Character c, IPacket p)
        {
            p.Encode<int>(c.ID);
            p.EncodeFixedString(c.Name, 13);

            p.Encode<byte>(c.Gender);
            p.Encode<byte>(c.Skin);
            p.Encode<int>(c.Face);
            p.Encode<int>(c.Hair);

            for (var i = 0; i < 3; i++)
                p.Encode<long>(0); // Pet stuff.

            p.Encode<byte>(c.Level);
            p.Encode(c.Job);
            p.Encode<short>(c.STR);
            p.Encode<short>(c.DEX);
            p.Encode<short>(c.INT);
            p.Encode<short>(c.LUK);
            p.Encode<int>(c.HP);
            p.Encode<int>(c.MaxHP);
            p.Encode<int>(c.MP);
            p.Encode<int>(c.MaxMP);

            p.Encode<short>(c.AP);
            if (SkillConstants.IsExtendSPJob(c.Job))
                c.EncodeExtendSP(p);
            else
                p.Encode<short>(c.SP);

            p.Encode<int>(c.EXP);
            p.Encode<short>(c.POP);

            p.Encode<int>(c.TempEXP);
            p.Encode<int>(c.FieldID);
            p.Encode<byte>(c.FieldPortal);
            p.Encode<int>(c.PlayTime);
            p.Encode<short>(c.SubJob);
        }

        public static void EncodeLook(this Character c, IPacket p)
        {
            p.Encode<byte>(c.Gender);
            p.Encode<byte>(c.Skin);
            p.Encode<int>(c.Face);

            p.Encode<bool>(false);
            p.Encode<int>(c.Hair);

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
                p.Encode<byte>((byte) Math.Abs(kv.Key));
                p.Encode<int>(kv.Value.TemplateID);
            });
            p.Encode<byte>(0xFF);

            unseen.ForEach(kv =>
            {
                p.Encode<byte>((byte) Math.Abs(kv.Key));
                p.Encode<int>(kv.Value.TemplateID);
            });
            p.Encode<byte>(0xFF);

            p.Encode<int>(inventory.Items.ContainsKey(-111)
                ? inventory.Items[-111].TemplateID
                : 0);

            for (var i = 0; i < 3; i++)
                p.Encode<int>(0);
        }

        public static void EncodeExtendSP(this Character c, IPacket p)
        {
            p.Encode<byte>((byte) c.ExtendSP.Count);
            c.ExtendSP.ForEach(kv =>
            {
                p.Encode<byte>(kv.Key);
                p.Encode<byte>(kv.Value);
            });
        }
    }
}