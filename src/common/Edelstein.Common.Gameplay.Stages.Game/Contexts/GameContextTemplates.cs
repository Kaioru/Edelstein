using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContextTemplates(
    ITemplateManager<IItemTemplate> Item
) : IGameContextTemplates;
