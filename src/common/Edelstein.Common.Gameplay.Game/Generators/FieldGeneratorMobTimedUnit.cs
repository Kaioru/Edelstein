using Edelstein.Common.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Generators;

public class FieldGeneratorMobTimedUnit : IFieldGeneratorUnit
{
    private readonly IField _field;
    private readonly IFieldTemplateLife _life;
    private readonly IMobTemplate _template;

    public FieldGeneratorMobTimedUnit(IField field, IFieldTemplateLife life, IMobTemplate template)
    {
        _field = field;
        _life = life;
        _template = template;
    }

    private DateTime NextRegen { get; set; }
    private IFieldMob? Mob { get; set; }


    public IPoint2D Position => _life.Position;

    public IFieldObject? Generate()
    {
        if (Check(DateTime.UtcNow))
            return new FieldMob(
                _template,
                _life.Position,
                _field.Template.Footholds.FindByID(_life.FootholdID),
                _life.IsFacingLeft
            );
        return null;
    }

    private bool Check(DateTime now)
    {
        if (Mob == null) return NextRegen < now;
        if (Mob.Field != null) return false;
        
        var random = new Random();
        var buffer = 7 * _life.MobTime / 10;
        var interval = TimeSpan.FromSeconds(
            13 * _life.MobTime / 10 + random.Next(buffer)
        );

        NextRegen = NextRegen.Add(interval);
        Mob = null;

        return false;

    }
}
