using Edelstein.Common.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Generators;

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

        NPC = new FieldNPC(
            _template,
            _field.Template.Footholds.FindBelow(_life.Position).FirstOrDefault()?.Line.AtX(_life.Position.X) ?? _life.Position,
            _field.Template.Footholds.FindByID(_life.FootholdID),
            _life.Bounds,
            _life.IsFacingLeft
        );
        return NPC;
    }
}
