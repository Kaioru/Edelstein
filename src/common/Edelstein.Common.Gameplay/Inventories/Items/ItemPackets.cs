using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Inventories.Items;

public static class ItemPackets
{
    public static void WriteItemData(this IPacketWriter writer, IItemSlot item)
    {
        switch (item)
        {
            case IItemSlotEquip equip:
                writer.WriteItemEquipData(equip);
                return;
            case IItemSlotBundle bundle:
                writer.WriteItemBundleData(bundle);
                return;
            case IItemSlotPet pet:
                writer.WriteItemPetData(pet);
                return;
            default:
                writer.WriteItemBase(item);
                return;
        }
    }

    private static void WriteItemBase(this IPacketWriter writer, IItemSlot item)
    {
        writer.WriteInt(item.ID);

        if (item is IItemSlotBase slot)
        {
            writer.WriteBool(slot.CashItemSN.HasValue);
            if (slot.CashItemSN.HasValue)
                writer.WriteLong(slot.CashItemSN.Value);
            writer.WriteDateTime(slot.DateExpire ?? DateTime.FromFileTimeUtc(150842304000000000));
        }
        else
        {
            writer.WriteBool(false);
            writer.WriteDateTime(DateTime.FromFileTimeUtc(150842304000000000));
        }
    }

    public static void WriteItemEquipData(this IPacketWriter writer, IItemSlotEquip equip)
    {
        writer.WriteByte(1);

        writer.WriteItemBase(equip);

        writer.WriteByte(equip.RUC);
        writer.WriteByte(equip.CUC);

        writer.WriteShort(equip.STR);
        writer.WriteShort(equip.DEX);
        writer.WriteShort(equip.INT);
        writer.WriteShort(equip.LUK);
        writer.WriteShort(equip.MaxHP);
        writer.WriteShort(equip.MaxMP);
        writer.WriteShort(equip.PAD);
        writer.WriteShort(equip.MAD);
        writer.WriteShort(equip.PDD);
        writer.WriteShort(equip.MDD);
        writer.WriteShort(equip.ACC);
        writer.WriteShort(equip.EVA);

        writer.WriteShort(equip.Craft);
        writer.WriteShort(equip.Speed);
        writer.WriteShort(equip.Jump);
        writer.WriteString(equip.Title);
        writer.WriteShort(equip.Attribute);

        writer.WriteByte(equip.LevelUpType);
        writer.WriteByte(equip.Level);
        writer.WriteInt(equip.EXP);
        writer.WriteInt(equip.Durability ?? 100);

        writer.WriteInt(equip.IUC);

        writer.WriteByte(equip.Grade);
        writer.WriteByte(equip.CHUC);

        writer.WriteShort(equip.Option1);
        writer.WriteShort(equip.Option2);
        writer.WriteShort(equip.Option3);
        writer.WriteShort(equip.Socket1);
        writer.WriteShort(equip.Socket2);

        writer.WriteLong(0); // TODO: cash sn
        writer.WriteLong(0);
        writer.WriteInt(0);
    }

    public static void WriteItemBundleData(this IPacketWriter writer, IItemSlotBundle bundle)
    {
        writer.WriteByte(2);

        writer.WriteItemBase(bundle);

        writer.WriteShort(bundle.Number);
        writer.WriteString(bundle.Title);
        writer.WriteShort(bundle.Attribute);

        if (bundle.ID / 10000 == 207 || bundle.ID / 10000 == 233) // TODO: constants, recharge items
            writer.WriteLong(0);
    }

    public static void WriteItemPetData(this IPacketWriter writer, IItemSlotPet pet)
    {
        writer.WriteByte(3);

        writer.WriteItemBase(pet);

        writer.WriteString(pet.PetName, 13);
        writer.WriteByte(pet.Level);
        writer.WriteShort(pet.Tameness);
        writer.WriteByte(pet.Repleteness);

        if (pet.DateDead != null) writer.WriteDateTime(pet.DateDead.Value);
        else writer.WriteLong(0);

        writer.WriteShort(pet.PetAttribute);
        writer.WriteShort(pet.PetSkill);
        writer.WriteInt(pet.RemainLife);
        writer.WriteShort(pet.Attribute);
    }
}
