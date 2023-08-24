using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Characters;

public static class CharacterPackets
{
    public static void WriteCharacterData(
        this IPacketWriter writer,
        ICharacter character,
        DbFlags flags = DbFlags.All
    )
    {
        writer.WriteLong((long)flags);
        writer.WriteByte(0);
        writer.WriteByte(0);

        if (flags.HasFlag(DbFlags.Character))
        {
            writer.WriteCharacterStats(character);

            writer.WriteByte(250); // nFriendMax
            writer.WriteBool(false);
        }

        if (flags.HasFlag(DbFlags.Money)) writer.WriteInt(character.Money);

        if (flags.HasFlag(DbFlags.InventorySize))
        {
            if (flags.HasFlag(DbFlags.ItemSlotEquip))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Equip]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotConsume))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Consume]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotInstall))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Install]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotEtc))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Etc]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotCash))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Cash]?.SlotMax ?? 24));
        }

        if (flags.HasFlag(DbFlags.AdminShopCount))
        {
            writer.WriteInt(0);
            writer.WriteInt(0);
        }

        if (flags.HasFlag(DbFlags.ItemSlotEquip))
        {
            var inventory = character.Inventories[ItemInventoryType.Equip]?.Items ?? new Dictionary<short, IItemSlot>();
            var equip = inventory.Where(kv => kv.Key >= 0);
            var equipped = inventory.Where(kv => kv.Key is >= -100 and < 0);
            var equipped2 = inventory.Where(kv => kv.Key is >= -1000 and < -100);
            var dragon = inventory.Where(kv => kv.Key is >= -1100 and < -1000);
            var mechanic = inventory.Where(kv => kv.Key is >= -1200 and < -1100);

            foreach (var items in new[] { equipped, equipped2, equip, dragon, mechanic })
            {
                foreach (var kv in items)
                {
                    writer.WriteShort((short)Math.Abs(kv.Key % 100));
                    writer.WriteItemData(kv.Value);
                }

                writer.WriteShort(0);
            }
        }

        foreach (var t in new List<(DbFlags, ItemInventoryType)>
                     {
                         (DbFlags.ItemSlotConsume, ItemInventoryType.Consume),
                         (DbFlags.ItemSlotInstall, ItemInventoryType.Install),
                         (DbFlags.ItemSlotEtc, ItemInventoryType.Etc),
                         (DbFlags.ItemSlotCash, ItemInventoryType.Cash)
                     }
                     .Where(t => flags.HasFlag(t.Item1)))
        {
            var items = character.Inventories[t.Item2]?.Items ?? new Dictionary<short, IItemSlot>();

            foreach (var kv in items)
            {
                writer.WriteByte((byte)kv.Key);
                writer.WriteItemData(kv.Value);
            }

            writer.WriteByte(0);
        }

        if (flags.HasFlag(DbFlags.SkillRecord)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.SkillCooltime)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.QuestRecord)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.QuestComplete)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.MinigameRecord)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.CoupleRecord))
        {
            writer.WriteShort(0); // Couple
            writer.WriteShort(0); // Friend
            writer.WriteShort(0); // Marriage
        }

        if (flags.HasFlag(DbFlags.MapTransfer))
        {
            for (var i = 0; i < 5; i++) writer.WriteInt(0);
            for (var i = 0; i < 10; i++) writer.WriteInt(0);
        }

        if (flags.HasFlag(DbFlags.NewYearCard)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.QuestRecordEx)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.WildHunterInfo))
            if (character.Job / 100 == 33)
            {
                writer.WriteByte(0);
                for (var i = 0; i < 5; i++) writer.WriteInt(0);
            }

        if (flags.HasFlag(DbFlags.QuestCompleteOld)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.VisitorLog)) writer.WriteShort(0);
    }

    public static void WriteCharacterStats(
        this IPacketWriter writer,
        ICharacter character
    )
    {
        writer.WriteInt(character.ID);
        writer.WriteString(character.Name, 13);

        writer.WriteByte(character.Gender);
        writer.WriteByte(character.Skin);
        writer.WriteInt(character.Face);
        writer.WriteInt(character.Hair);

        writer.WriteLong(0);
        writer.WriteLong(0);
        writer.WriteLong(0);

        writer.WriteByte(character.Level);
        writer.WriteShort(character.Job);
        writer.WriteShort(character.STR);
        writer.WriteShort(character.DEX);
        writer.WriteShort(character.INT);
        writer.WriteShort(character.LUK);
        writer.WriteInt(character.HP);
        writer.WriteInt(character.MaxHP);
        writer.WriteInt(character.MP);
        writer.WriteInt(character.MaxMP);

        writer.WriteShort(character.AP);
        writer.WriteShort(character.SP); // TODO: extendSP

        writer.WriteInt(character.EXP);
        writer.WriteShort(character.POP);

        writer.WriteInt(character.TempEXP);
        writer.WriteInt(character.FieldID);
        writer.WriteByte(character.FieldPortal);
        writer.WriteInt(character.PlayTime);
        writer.WriteShort(character.SubJob);
    }

    public static void WriteCharacterLooks(
        this IPacketWriter writer,
        ICharacter character
    )
    {
        writer.WriteByte(character.Gender);
        writer.WriteByte(character.Skin);
        writer.WriteInt(character.Face);

        writer.WriteBool(false);
        writer.WriteInt(character.Hair);

        var inventory = character.Inventories[ItemInventoryType.Equip];
        var equipped = inventory?.Items
            .Where(kv => kv.Key < 0)
            .Select(kv => Tuple.Create(Math.Abs(kv.Key), kv.Value))
            .ToList() ?? new List<Tuple<short, IItemSlot>>();
        var stickers = equipped
            .Where(t => t.Item1 > 100)
            .Select(t => Tuple.Create(t.Item1 - 100, t.Item2))
            .ToDictionary(
                kv => kv.Item1,
                kv => kv.Item2
            );
        var hidden = new Dictionary<short, IItemSlot>();

        foreach (var tuple in equipped.Where(t => t.Item1 < 100))
            if (!stickers.ContainsKey(tuple.Item1))
                stickers[tuple.Item1] = tuple.Item2;
            else
                hidden[tuple.Item1] = tuple.Item2;

        foreach (var kv in stickers)
        {
            writer.WriteByte((byte)kv.Key);
            writer.WriteInt(kv.Value.ID);
        }

        writer.WriteByte(0xFF);

        foreach (var kv in hidden)
        {
            writer.WriteByte((byte)kv.Key);
            writer.WriteInt(kv.Value.ID);
        }

        writer.WriteByte(0xFF);

        var item = character.Inventories[ItemInventoryType.Equip]?.Items ?? new Dictionary<short, IItemSlot>();

        writer.WriteInt(item.TryGetValue(-111, out var v1) 
            ? v1.ID
            : item.TryGetValue(-11, out var v2) 
                ? v2.ID
                : 0
        );

        for (var i = 0; i < 3; i++)
            writer.WriteInt(0);
    }
}
