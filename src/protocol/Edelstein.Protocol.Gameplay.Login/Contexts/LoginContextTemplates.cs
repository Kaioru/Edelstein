using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextTemplates(
    ITemplateManager<IItemTemplate> Items
);
