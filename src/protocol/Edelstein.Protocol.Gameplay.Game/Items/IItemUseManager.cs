using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public interface IItemUseManager<TContext, TTemplate> : 
    IPipework<TContext>
    where TContext : IItemUse<TTemplate>
    where TTemplate : IItemTemplate
{
    Task<TContext?> Process(int templateID);
    Task Use(IFieldUser user, ItemInventoryType type, short slot, bool exclRequest = false);
}
