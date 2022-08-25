using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Generators;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public class FieldMobNormalGenerator : IFieldGenerator
{
    private readonly IField _field;
    private readonly IFieldTemplateLife _life;
    private readonly IMobTemplate _template;

    public FieldMobNormalGenerator(IField field, IFieldTemplateLife life, IMobTemplate template)
    {
        _field = field;
        _life = life;
        _template = template;
    }

    private IFieldMob? Mob { get; set; }

    public bool IsGenerateOnInit => false;

    public IFieldObject? Generate()
    {
        // TODO: rework mob generators to match
        if (Mob != null) return null;

        Mob = _field.Manager.CreateMob(
            _template,
            _life.Position,
            _field.Template.Footholds.FindByID(_life.FootholdID),
            _life.IsFacingLeft
        );
        return Mob;
    }
}
