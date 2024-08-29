namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;

public enum ModifyInventoryOperationType : byte
{
    Add = 0x0,
    UpdateNumber = 0x1,
    Move = 0x2,
    Remove = 0x3,
    UpdateEXP = 0x4
}
