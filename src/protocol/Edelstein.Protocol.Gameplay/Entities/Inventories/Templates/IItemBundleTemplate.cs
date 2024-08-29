namespace Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;

public interface IItemBundleTemplate : IItemTemplate
{
    double UnitPrice { get; }
    
    int ReqLevel { get; }
    int IncPAD { get; }
    
    short MaxPerSlot { get; }
}
