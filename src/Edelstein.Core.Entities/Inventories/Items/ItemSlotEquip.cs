namespace Edelstein.Entities.Inventories.Items
{
    public class ItemSlotEquip : ItemSlot
    {
        public byte RUC { get; set; }
        public byte CUC { get; set; }

        public short STR { get; set; }
        public short DEX { get; set; }
        public short INT { get; set; }
        public short LUK { get; set; }
        public short MaxHP { get; set; }
        public short MaxMP { get; set; }
        public short PAD { get; set; }
        public short MAD { get; set; }
        public short PDD { get; set; }
        public short MDD { get; set; }
        public short ACC { get; set; }
        public short EVA { get; set; }

        public short Craft { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }

        public string Title { get; set; }
        public short Attribute { get; set; }
        public byte LevelUpType { get; set; }
        public byte Level { get; set; }
        public int EXP { get; set; }
        public int Durability { get; set; }

        public int IUC { get; set; }

        public byte Grade { get; set; }
        public byte CHUC { get; set; }

        public short Option1 { get; set; }
        public short Option2 { get; set; }
        public short Option3 { get; set; }
        public short Socket1 { get; set; }
        public short Socket2 { get; set; }
    }
}