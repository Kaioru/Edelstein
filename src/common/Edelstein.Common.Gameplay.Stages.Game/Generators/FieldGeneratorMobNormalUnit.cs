using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public class FieldGeneratorMobNormalUnit : IFieldGeneratorUnit
{
    private readonly IField _field;
    private readonly IFieldTemplateLife _life;
    private readonly IMobTemplate _template;

    public FieldGeneratorMobNormalUnit(IField field, IFieldTemplateLife life, IMobTemplate template)
    {
        _field = field;
        _life = life;
        _template = template;
    }

    public IPoint2D Position => _life.Position;

    public IFieldObject Generate() =>
        _field.Manager.CreateMob(
            _template,
            _life.Position,
            _field.Template.Footholds.FindByID(_life.FootholdID),
            _life.IsFacingLeft
        );
}
