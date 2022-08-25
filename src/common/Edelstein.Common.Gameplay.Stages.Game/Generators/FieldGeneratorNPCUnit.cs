using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public class FieldGeneratorNPCUnit : IFieldGeneratorUnit
{
    private readonly IField _field;
    private readonly IFieldTemplateLife _life;
    private readonly INPCTemplate _template;

    public FieldGeneratorNPCUnit(IField field, IFieldTemplateLife life, INPCTemplate template)
    {
        _field = field;
        _life = life;
        _template = template;
    }

    private IFieldNPC? NPC { get; set; }

    public IPoint2D Position => _life.Position;

    public IFieldObject? Generate()
    {
        if (NPC != null) return null;

        NPC = _field.Manager.CreateNPC(
            _template,
            _life.Position,
            _field.Template.Footholds.FindByID(_life.FootholdID),
            _life.Bounds,
            _life.IsFacingLeft
        );
        return NPC;
    }
}
