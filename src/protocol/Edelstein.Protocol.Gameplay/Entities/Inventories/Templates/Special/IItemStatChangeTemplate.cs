namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;

public interface IItemStatChangeTemplate : IItemBundleTemplate
{
    int? HP { get; }
    int? MP { get; }
    int? HPr { get; }
    int? MPr { get; }
}
