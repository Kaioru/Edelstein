namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldControllable : IFieldLife
{
    IFieldController? Controller { get; }

    Task Control(IFieldController? controller = null);
}
