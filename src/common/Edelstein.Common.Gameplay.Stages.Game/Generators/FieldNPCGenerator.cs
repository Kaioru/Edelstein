using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Generators;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators;

public class FieldNPCGenerator : IFieldGenerator
{
    private readonly IField _field;
    private readonly IFieldTemplateLife _life;
    private readonly INPCTemplate _template;


    public FieldNPCGenerator(IField field, IFieldTemplateLife life, INPCTemplate template)
    {
        _field = field;
        _life = life;
        _template = template;
    }

    private IFieldNPC? NPC { get; set; }

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
