namespace Edelstein.Protocol.Gameplay.Inventories.Templates;

public interface IItemBundleTemplate : IItemTemplate
{
    double UnitPrice { get; }
    short MaxPerSlot { get; }
}
