using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Generators;

namespace Edelstein.Common.Gameplay.Game.Generators;

public class FieldGeneratorNPC : IFieldGenerator
{
    private readonly IField _field;
    private readonly ICollection<IFieldGeneratorUnit> _units;

    public FieldGeneratorNPC(string id, IField field, ICollection<IFieldGeneratorUnit> units)
    {
        ID = id;
        _field = field;
        _units = units;
    }
    
    public string ID { get; }

    public async Task Generate()
    {
        foreach (var unit in _units)
        {
            var obj = unit.Generate();
            if (obj == null) continue;
            await _field.Enter(obj);
        }
    }
}
