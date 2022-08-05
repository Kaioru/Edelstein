namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldControllable : IFieldObject
{
    IFieldController? Controller { get; }

    Task Control(IFieldController? controller = null);
}
