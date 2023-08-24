using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

public interface IItemStringTemplate : ITemplate
{
    string Name { get; }
    string Desc { get; }
}
