﻿using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories;

namespace Edelstein.Common.Gameplay.Users
{
    public static class CharacterHelpers
    {
        public static byte GetExtendSP(this Character c, byte jobLevel)
               => c.ExtendSP.ContainsKey(jobLevel)
                   ? c.ExtendSP[jobLevel]
                   : (byte)0;

        public static void SetExtendSP(this Character c, byte jobLevel, byte sp)
            => c.ExtendSP[jobLevel] = sp;

        public static int GetSkillLevel(this Character c, int skill)
            => c.SkillRecord.ContainsKey(skill)
                ? c.SkillRecord[skill].Level
                : 0;

        public static int GetSkillMasterLevel(this Character c, int skill)
            => c.SkillRecord.ContainsKey(skill)
                ? c.SkillRecord[skill].MasterLevel ?? 0
                : 0;

        public static bool HasSlotFor(this Character c, AbstractItemSlot item)
        {
            return HasSlotFor(
                c,
                (ItemInventoryType)(item.TemplateID / 1000000),
                item
            );
        }

        public static bool HasSlotFor(this Character c, ItemInventoryType type, AbstractItemSlot item)
            => AvailableSlotsFor(c, type) > 0;

        public static bool HasSlotFor(this Character c, ICollection<AbstractItemSlot> items)
        {
            return items
                .GroupBy(i => (ItemInventoryType)(i.TemplateID / 1000000))
                .All(g => AvailableSlotsFor(c, g.Key) >= g.Count());
        }

        public static int AvailableSlotsFor(this Character c, ItemInventoryType type)
        {
            var inventory = c.Inventories[type];

            var usedSlots = inventory.Items
                .Select(kv => (int)kv.Key)
                .Where(s => s > 0)
                .Where(s => s <= inventory.SlotMax)
                .ToList();
            var unusedSlots = Enumerable.Range(1, inventory.SlotMax)
                .Except(usedSlots)
                .ToList();
            return unusedSlots.Count;
        }

        public static bool HasItem(this Character c, int templateID)
        {
            return HasItem(
                c,
                (ItemInventoryType)(templateID / 1000000),
                templateID
            );
        }

        public static bool HasItem(this Character c, ItemInventoryType type, int templateID)
        {
            return GetItemCount(c, type, templateID) > 0;
        }

        public static int GetItemCount(this Character c, int templateID)
        {
            return GetItemCount(
                c,
                (ItemInventoryType)(templateID / 1000000),
                templateID
            );
        }

        public static int GetItemCount(this Character c, ItemInventoryType type, int templateID)
        {
            var inventory = c.Inventories[type];

            return inventory.Items
                .Select(kv => kv.Value)
                .Where(i => i.TemplateID == templateID)
                .Select(i =>
                {
                    if (i is ItemSlotBundle bundle)
                        return bundle.Number;
                    return 1;
                })
                .Sum();
        }
    }
}
