using Edelstein.Protocol.Gameplay.Stages.Game.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldFactory
{
    IField CreateField(IFieldTemplate template);
}
