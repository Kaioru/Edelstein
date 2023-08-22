using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextTemplates(
    ITemplateManager<IItemTemplate> Item
);
