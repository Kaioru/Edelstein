using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextTemplates
{
    ITemplateManager<IItemTemplate> Item { get; }
    ITemplateManager<IFieldTemplate> Field { get; }
    ITemplateManager<INPCTemplate> NPC { get; }
}
