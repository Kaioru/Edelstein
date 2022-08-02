using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextTemplates(
    ITemplateManager<IWorldTemplate> World,
    ITemplateManager<IItemTemplate> Item
) : ILoginContextTemplates;
