using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Common.Gameplay.Characters;

public static class CharacterPackets
{
    public static void WriteCharacterData(
        this IPacketWriter writer,
        ICharacter c,
        CharacterFlags flags = CharacterFlags.All
    )
    {
        writer.WriteLong((long)flags);
        writer.WriteByte(0);
        writer.WriteByte(0);

        if (flags.HasFlag(CharacterFlags.Character))
        {
            writer.WriteCharacterStats(c);

            writer.WriteByte(250); // nFriendMax
            writer.WriteBool(false);
        }

        if (flags.HasFlag(CharacterFlags.Money)) writer.WriteInt(c.Money);

        if (flags.HasFlag(CharacterFlags.InventorySize))
        {
            if (flags.HasFlag(CharacterFlags.ItemSlotEquip))
                writer.WriteByte((byte)c.Inventories[ItemInventoryType.Equip].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotConsume))
                writer.WriteByte((byte)c.Inventories[ItemInventoryType.Consume].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotInstall))
                writer.WriteByte((byte)c.Inventories[ItemInventoryType.Install].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotEtc))
                writer.WriteByte((byte)c.Inventories[ItemInventoryType.Etc].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotCash))
                writer.WriteByte((byte)c.Inventories[ItemInventoryType.Cash].SlotMax);
        }

        if (flags.HasFlag(CharacterFlags.AdminShopCount))
        {
            writer.WriteInt(0);
            writer.WriteInt(0);
        }

        if (flags.HasFlag(CharacterFlags.ItemSlotEquip))
        {
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
        }

        foreach (var t in new List<(CharacterFlags, ItemInventoryType)>
                     {
                         (CharacterFlags.ItemSlotConsume, ItemInventoryType.Consume),
                         (CharacterFlags.ItemSlotInstall, ItemInventoryType.Install),
                         (CharacterFlags.ItemSlotEtc, ItemInventoryType.Etc),
                         (CharacterFlags.ItemSlotCash, ItemInventoryType.Cash)
                     }
                     .Where(t => flags.HasFlag(t.Item1)))
            writer.WriteByte(0);

        if (flags.HasFlag(CharacterFlags.SkillRecord)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.SkillCooltime)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.QuestRecord)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.QuestComplete)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.MinigameRecord)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.CoupleRecord))
        {
            writer.WriteShort(0); // Couple
            writer.WriteShort(0); // Friend
            writer.WriteShort(0); // Marriage
        }

        if (flags.HasFlag(CharacterFlags.MapTransfer))
        {
            for (var i = 0; i < 5; i++) writer.WriteInt(0);
            for (var i = 0; i < 10; i++) writer.WriteInt(0);
        }

        if (flags.HasFlag(CharacterFlags.NewYearCard)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.QuestRecordEx)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.WildHunterInfo))
            if (c.Job / 100 == 33)
            {
                writer.WriteByte(0);
                for (var i = 0; i < 5; i++) writer.WriteInt(0);
            }

        if (flags.HasFlag(CharacterFlags.QuestCompleteOld)) writer.WriteShort(0);

        if (flags.HasFlag(CharacterFlags.VisitorLog)) writer.WriteShort(0);
    }

    public static void WriteCharacterStats(
        this IPacketWriter writer,
        ICharacter c
    )
    {
        writer.WriteInt(c.ID);
        writer.WriteString(c.Name, 13);

        writer.WriteByte(c.Gender);
        writer.WriteByte(c.Skin);
        writer.WriteInt(c.Face);
        writer.WriteInt(c.Hair);

        foreach (var sn in c.Pets)
            writer.WriteLong(sn);

        writer.WriteByte(c.Level);
        writer.WriteShort(c.Job);
        writer.WriteShort(c.STR);
        writer.WriteShort(c.DEX);
        writer.WriteShort(c.INT);
        writer.WriteShort(c.LUK);
        writer.WriteInt(c.HP);
        writer.WriteInt(c.MaxHP);
        writer.WriteInt(c.MP);
        writer.WriteInt(c.MaxMP);

        writer.WriteShort(c.AP);
        writer.WriteShort(c.SP); // TODO: extendSP

        writer.WriteInt(c.EXP);
        writer.WriteShort(c.POP);

        writer.WriteInt(c.TempEXP);
        writer.WriteInt(c.FieldID);
        writer.WriteByte(c.FieldPortal);
        writer.WriteInt(c.PlayTime);
        writer.WriteShort(c.SubJob);
    }

    public static void WriteCharacterLooks(
        this IPacketWriter writer,
        ICharacter c
    )
    {
        writer.WriteByte(c.Gender);
        writer.WriteByte(c.Skin);
        writer.WriteInt(c.Face);

        writer.WriteBool(false);
        writer.WriteInt(c.Hair);

        writer.WriteByte(0xFF);
        writer.WriteByte(0xFF);
        writer.WriteInt(0);

        for (var i = 0; i < 3; i++)
            writer.WriteInt(0);
    }
}
