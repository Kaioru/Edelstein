namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

public interface IItemBundleTemplate : IItemTemplate
{
    double UnitPrice { get; }
    
    int ReqLevel { get; }
    int IncPAD { get; }
    
    short MaxPerSlot { get; }
}
