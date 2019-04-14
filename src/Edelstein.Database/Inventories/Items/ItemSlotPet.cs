using System;

namespace Edelstein.Database.Inventories.Items
{
    public class ItemSlotPet
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