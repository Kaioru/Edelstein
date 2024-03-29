﻿using Edelstein.Protocol.Gameplay.Login.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextTemplates(
    ITemplateManager<IItemTemplate> Item,
    ITemplateManager<IItemStringTemplate> ItemString,
    ITemplateManager<ISkillTemplate> Skill,
    ITemplateManager<ISkillStringTemplate> SkillString,
    ITemplateManager<IWorldTemplate> World
);
