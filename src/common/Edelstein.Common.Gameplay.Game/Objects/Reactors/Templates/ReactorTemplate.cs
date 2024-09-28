using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Reactors.Templates;

public record ReactorTemplate : IReactorTemplate
{
    public int ID { get; }
    
    public ReactorTemplate(int id, IDataNode node)
    {
        ID = id;
    }
}
