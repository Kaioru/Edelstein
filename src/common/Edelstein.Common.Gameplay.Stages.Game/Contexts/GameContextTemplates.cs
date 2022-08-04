using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Continents.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContextTemplates(
    ITemplateManager<IItemTemplate> Item,
    ITemplateManager<IFieldTemplate> Field,
    ITemplateManager<IContiMoveTemplate> ContiMove,
    ITemplateManager<INPCTemplate> NPC
) : IGameContextTemplates;
