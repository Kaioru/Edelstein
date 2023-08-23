using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Generators;

public interface IFieldGenerator : IIdentifiable<string>
{
    IEnumerable<IFieldObject> Generate();
}
