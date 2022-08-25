namespace Edelstein.Protocol.Gameplay.Inventories.Items;

public interface IItemSlotEquip : IItemSlotBase
{
    byte RUC { get; set; }
    byte CUC { get; set; }

    short STR { get; set; }
    short DEX { get; set; }
    short INT { get; set; }
    short LUK { get; set; }
    short MaxHP { get; set; }
    short MaxMP { get; set; }
    short PAD { get; set; }
    short MAD { get; set; }
    short PDD { get; set; }
    short MDD { get; set; }
    short ACC { get; set; }
    short EVA { get; set; }

    short Craft { get; set; }
    short Speed { get; set; }
    short Jump { get; set; }

    string? Title { get; set; }
    short Attribute { get; set; }
    byte LevelUpType { get; set; }
    byte Level { get; set; }
    int EXP { get; set; }
    int? Durability { get; set; }

    int IUC { get; set; }

    byte Grade { get; set; }
    byte CHUC { get; set; }

    short Option1 { get; set; }
    short Option2 { get; set; }
    short Option3 { get; set; }
    short Socket1 { get; set; }
    short Socket2 { get; set; }
}
