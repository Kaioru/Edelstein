using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Generators;

public interface IFieldGenerator
{
    bool IsGenerateOnInit { get; }

    IFieldObject? Generate();
}
