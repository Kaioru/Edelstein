using Edelstein.Common.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Generators;

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
        new FieldMob(
            _template,
            _life.Position,
            _field.Template.Footholds.FindByID(_life.FootholdID),
            _field.Template.Footholds.FindByID(_life.FootholdID),
            _life.IsFacingLeft
        );
}
