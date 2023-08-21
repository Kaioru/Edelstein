namespace Edelstein.Protocol.Gameplay.Models.Inventories;

public interface IItemTrunk : IItemInventory
{
    int Money { get; set; }
}
