using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Characters;

public static class CharacterPackets
{
    public static void WriteCharacterData(
        this IPacketWriter writer,
        ICharacter character,
        CharacterFlags flags = CharacterFlags.All
    )
    {
        writer.WriteLong((long)flags);
        writer.WriteByte(0);

        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteInt(0);

        writer.WriteByte(0);
        writer.WriteByte(0);
        writer.WriteInt(0);
        writer.WriteByte(0);

        if (flags.HasFlag(CharacterFlags.Character))
        {
            writer.WriteCharacterStats(character);

            writer.WriteByte(250); // nFriendMax

            writer.WriteBool(false);
            writer.WriteBool(false);
            writer.WriteBool(false);
        }

        if (flags.HasFlag(CharacterFlags.Money))
            writer.WriteLong(character.Money);

        if (flags.HasFlag(CharacterFlags.ItemSlotConsume)) writer.WriteInt(0);
        if (flags.HasFlag(CharacterFlags.ItemSlotConsume)) writer.WriteInt(0);

        if (flags.HasFlag(CharacterFlags.InventorySize))
        {
            if (flags.HasFlag(CharacterFlags.ItemSlotEquip))
                writer.WriteByte((byte)character.Inventories[ItemInventoryType.Equip].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotConsume))
                writer.WriteByte((byte)character.Inventories[ItemInventoryType.Consume].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotInstall))
                writer.WriteByte((byte)character.Inventories[ItemInventoryType.Install].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotEtc))
                writer.WriteByte((byte)character.Inventories[ItemInventoryType.Etc].SlotMax);
            if (flags.HasFlag(CharacterFlags.ItemSlotCash))
                writer.WriteByte((byte)character.Inventories[ItemInventoryType.Cash].SlotMax);
        }

        if (flags.HasFlag(CharacterFlags.ItemSlotEquip))
        {
            writer.WriteByte(0);

            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
        }

        if (flags.HasFlag(CharacterFlags.ItemSlotConsume)) writer.WriteByte(0);
        if (flags.HasFlag(CharacterFlags.ItemSlotInstall)) writer.WriteByte(0);
        if (flags.HasFlag(CharacterFlags.ItemSlotEtc)) writer.WriteByte(0);
        if (flags.HasFlag(CharacterFlags.ItemSlotCash)) writer.WriteByte(0);

        if (flags.HasFlag(CharacterFlags.ItemSlotConsume)) writer.WriteInt(0);
        if (flags.HasFlag(CharacterFlags.ItemSlotInstall)) writer.WriteInt(0);
        if (flags.HasFlag(CharacterFlags.ItemSlotEtc)) writer.WriteInt(0);
        if (flags.HasFlag(CharacterFlags.ItemSlotCash)) writer.WriteInt(0);

        writer.WriteInt(0);
        writer.WriteByte(0);

        if (flags.HasFlag(CharacterFlags.Character))
        {
            writer.WriteInt(0);
            writer.WriteInt(0);
        }

        if (flags.HasFlag(CharacterFlags.Money))
        {
            writer.WriteBool(false);
            writer.WriteShort(0);
        }

        writer.WriteShort(0);

        writer.WriteBool(false);
        writer.WriteInt(0);

        writer.WriteBytes(new byte[32]);
    }

    public static void WriteCharacterStats(
        this IPacketWriter writer,
        ICharacter character
    )
    {
        writer.WriteInt(character.ID);
        writer.WriteInt(character.ID);
        writer.WriteInt(0);
        writer.WriteString(character.Name, 13);

        writer.WriteByte(character.Gender);
        writer.WriteByte(character.Skin);
        writer.WriteInt(character.Face);
        writer.WriteInt(character.Hair);
        writer.WriteByte(0);
        writer.WriteByte(0);
        writer.WriteByte(0);

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
        writer.WriteByte(0); // TODO: extendSP

        writer.WriteLong(character.EXP);
        writer.WriteInt(character.POP);

        writer.WriteInt(0);
        writer.WriteInt(character.TempEXP);
        writer.WriteInt(character.FieldID);
        writer.WriteByte(character.FieldPortal);
        writer.WriteInt(character.PlayTime);
        writer.WriteShort(character.SubJob);

        writer.WriteByte(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteInt(0);

        // NonCombatStatDay
        writer.WriteShort(0);
        writer.WriteShort(0);
        writer.WriteShort(0);
        writer.WriteShort(0);
        writer.WriteShort(0);
        writer.WriteShort(0);
        writer.WriteByte(0);
        writer.WriteLong(0);

        writer.WriteInt(0);
        writer.WriteByte(0);
        writer.WriteInt(0);
        writer.WriteByte(2);
        writer.WriteByte(0);

        writer.WriteInt(0);
        writer.WriteByte(0);
        writer.WriteLong(0);
        writer.WriteInt(0);
        writer.WriteByte(0);

        for (var i = 0; i < 9; i++)
        {
            writer.WriteInt(0);
            writer.WriteByte(0);
            writer.WriteInt(0);
        }

        writer.WriteLong(0);
        writer.WriteBool(false);
    }

    public static void WriteCharacterLooks(
        this IPacketWriter writer,
        ICharacter character
    )
    {
        writer.WriteByte(character.Gender);
        writer.WriteByte(character.Skin);
        writer.WriteInt(character.Face);
        writer.WriteInt(character.Job);
        writer.WriteBool(false);
        writer.WriteInt(character.Hair);

        writer.WriteByte(0xFF);
        writer.WriteByte(0xFF);
        writer.WriteByte(0xFF);

        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
        writer.WriteBool(false);

        for (var i = 0; i < 3; i++)
            writer.WriteInt(0);

        writer.WriteByte(0);
        writer.WriteByte(0);
    }
}
