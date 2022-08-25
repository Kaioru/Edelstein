using Edelstein.Protocol.Gameplay.Stages.Game.Generators;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public class FieldGeneratorNPC : IFieldGenerator
{
    private readonly ICollection<IFieldGeneratorUnit> _units;

    public FieldGeneratorNPC(ICollection<IFieldGeneratorUnit> units) => _units = units;

    public IEnumerable<IFieldObject> Generate()
    {
        foreach (var unit in _units)
        {
            var obj = unit.Generate();
            if (obj == null) continue;
            yield return obj;
        }
    }
}
