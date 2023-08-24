using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Generators;

namespace Edelstein.Common.Gameplay.Game.Generators;

public class FieldGeneratorRegistry : Repository<string, IFieldGenerator>, IFieldGeneratorRegistry
{
}
