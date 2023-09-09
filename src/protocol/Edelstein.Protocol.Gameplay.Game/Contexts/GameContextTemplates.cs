using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextTemplates(
    ITemplateManager<IItemTemplate> Item,
    ITemplateManager<IItemStringTemplate> ItemString,
    ITemplateManager<ISkillTemplate> Skill,
    ITemplateManager<ISkillStringTemplate> SkillString,
    ITemplateManager<IFieldTemplate> Field,
    ITemplateManager<IFieldStringTemplate> FieldString,
    ITemplateManager<INPCTemplate> NPC,
    ITemplateManager<IMobTemplate> Mob,
    ITemplateManager<IContiMoveTemplate> ContiMove
);
