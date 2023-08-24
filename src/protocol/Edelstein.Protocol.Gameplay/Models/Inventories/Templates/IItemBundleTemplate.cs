namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

public interface IItemBundleTemplate : IItemTemplate
{
    double UnitPrice { get; }
    short MaxPerSlot { get; }
}
