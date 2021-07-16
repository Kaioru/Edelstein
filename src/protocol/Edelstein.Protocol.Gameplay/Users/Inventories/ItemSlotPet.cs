using System;

namespace Edelstein.Protocol.Gameplay.Users.Inventories
{
    public class ItemSlotPet : AbstractItemSlot
    {
        public string PetName { get; set; }

        public byte Level { get; set; }
        public short Tameness { get; set; }
        public byte Repleteness { get; set; }

        public DateTime? DateDead { get; set; }

        public short PetAttribute { get; set; }
        public short PetSkill { get; set; }

        public int RemainLife { get; set; }

        public short Attribute { get; set; }
    }
}
