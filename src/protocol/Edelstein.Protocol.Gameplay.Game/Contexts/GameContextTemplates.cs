using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextTemplates(
    ITemplateManager<IItemTemplate> Items,
    ITemplateManager<IFieldTemplate> Fields   
);
