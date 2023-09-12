namespace Edelstein.Protocol.Gameplay.Game.Objects.Dragon;

public interface IFieldDragon : 
    IFieldLife<IFieldDragonMovePath, IFieldDragonMoveAction>, 
    IFieldObjectOwned
{
    short JobCode { get; }
}
