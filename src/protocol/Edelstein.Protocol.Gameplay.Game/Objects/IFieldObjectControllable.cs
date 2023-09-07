namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectControllable : IFieldObject
{
    IFieldObjectController? Controller { get; }

    Task Control(IFieldObjectController? controller = null);
}
