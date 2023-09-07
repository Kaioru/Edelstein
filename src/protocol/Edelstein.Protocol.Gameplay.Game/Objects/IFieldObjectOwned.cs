using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectOwned : IFieldObject
{
    IFieldUser Owner { get; }
}
