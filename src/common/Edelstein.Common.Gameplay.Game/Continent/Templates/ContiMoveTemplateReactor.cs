using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;

namespace Edelstein.Common.Gameplay.Game.Continent.Templates;

public class ContiMoveTemplateReactor : IContiMoveTemplateReactor
{
    public string Name { get; }
    public int StateOnStart { get; }
    public int StateOnEnd { get; }
    
    public ContiMoveTemplateReactor(IDataNode reactor)
    {
        Name = reactor.ResolveString("name") ?? "NO-NAME";
        StateOnStart = reactor.ResolveInt("stateOnStart") ?? 0;
        StateOnEnd = reactor.ResolveInt("stateOnEnd") ?? 0;
    }
}
