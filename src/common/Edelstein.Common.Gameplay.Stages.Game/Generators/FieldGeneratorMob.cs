using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Generators;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public class FieldGeneratorMob : IFieldGenerator
{
    private readonly IField _field;
    private readonly ICollection<IFieldGeneratorUnit> _units;

    public FieldGeneratorMob(IField field, ICollection<IFieldGeneratorUnit> units)
    {
        _field = field;
        _units = units;
    }

    public IEnumerable<IFieldObject> Generate()
    {
        var random = new Random();
        var userCount = _field.GetPool(FieldObjectType.User)?.Objects.Count ?? 0;
        var mobCount = _field.GetPool(FieldObjectType.Mob)?.Objects.Count ?? 0;
        var mobCapacity = _field.Template.MobCapacityMin;

        if (userCount > _field.Template.MobCapacityMin / 2)
            mobCapacity += (_field.Template.MobCapacityMax - _field.Template.MobCapacityMin) *
                           (2 * userCount - _field.Template.MobCapacityMin) /
                           (3 * _field.Template.MobCapacityMin);

        mobCapacity = Math.Min(mobCapacity, _field.Template.MobCapacityMax);

        var mobGenCount = mobCapacity - mobCount;

        if (mobGenCount == 0) yield break;

        foreach (var unit in _units
                     .OrderByDescending(u => random.Next())
                     .Take(mobGenCount))
        {
            var obj = unit.Generate();
            if (obj == null) continue;
            yield return obj;
        }
    }
}
