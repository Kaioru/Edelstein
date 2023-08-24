using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Templates;

public interface IFieldStringTemplate : ITemplate
{
    string MapName { get; }
    string StreetName { get; }
}
