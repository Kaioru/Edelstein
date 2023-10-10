using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates.Sets;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextTemplates(
    ITemplateManager<IItemTemplate> Item,
    ITemplateManager<IItemStringTemplate> ItemString,
    ITemplateManager<IItemOptionTemplate> ItemOption,
    ITemplateManager<IItemSetTemplate> ItemSet,
    ITemplateManager<ISkillTemplate> Skill,
    ITemplateManager<ISkillStringTemplate> SkillString,
    ITemplateManager<IQuestTemplate> Quest,
    ITemplateManager<IFieldTemplate> Field,
    ITemplateManager<IFieldStringTemplate> FieldString,
    ITemplateManager<INPCTemplate> NPC,
    ITemplateManager<INPCStringTemplate> NPCString,
    ITemplateManager<IMobTemplate> Mob,
    ITemplateManager<IMobStringTemplate> MobString,
    ITemplateManager<IContiMoveTemplate> ContiMove
);
