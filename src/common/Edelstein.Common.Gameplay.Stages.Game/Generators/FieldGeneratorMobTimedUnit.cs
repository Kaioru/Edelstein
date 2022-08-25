using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

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
            return _field.Manager.CreateMob(
                _template,
                _life.Position,
                _field.Template.Footholds.FindByID(_life.FootholdID),
                _life.IsFacingLeft
            );
        return null;
    }

    private bool Check(DateTime now)
    {
        if (Mob != null)
        {
            if (Mob.Field == null)
            {
                var random = new Random();
                var buffer = 7 * _life.MobTime / 10;
                var interval = TimeSpan.FromSeconds(
                    13 * _life.MobTime / 10 + random.Next(buffer)
                );

                NextRegen = NextRegen.Add(interval);
                Mob = null;
            }

            return false;
        }

        return NextRegen < now;
    }
}
