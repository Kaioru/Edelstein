using Edelstein.Protocol.Gameplay.Game.Generators;
using Edelstein.Protocol.Gameplay.Game.Objects;

namespace Edelstein.Common.Gameplay.Game.Generators;

public class FieldGeneratorNPC : IFieldGenerator
{
    public string ID { get; }
    private readonly ICollection<IFieldGeneratorUnit> _units;

    public FieldGeneratorNPC(string id, ICollection<IFieldGeneratorUnit> units)
    {
        ID = id;
        _units = units;
    }

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
