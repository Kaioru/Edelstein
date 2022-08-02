namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldController
{
    ICollection<IFieldControllable> Controlled { get; }
}
