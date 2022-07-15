namespace Edelstein.Protocol.Gameplay.Inventories.Items;

public interface IItemSlotPet : IItemSlot
{
    string PetName { get; set; }
    short PetAttribute { get; set; }
    short PetSkill { get; set; }

    byte Level { get; set; }
    short Tameness { get; set; }
    byte Repleteness { get; set; }

    DateTime? DateDead { get; set; }

    int RemainLife { get; set; }

    short Attribute { get; set; }
}
