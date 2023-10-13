using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectOwned : IFieldObject
{
    IFieldUser Owner { get; }
    
    Task Move(IPoint2D position, IFieldFoothold? foothold);
}
