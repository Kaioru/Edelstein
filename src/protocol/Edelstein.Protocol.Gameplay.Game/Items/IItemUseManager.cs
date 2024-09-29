using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public interface IItemUseManager<out TContext, TInfo, TTemplate> : 
    IPipework<TContext>
    where TContext : IItemUseManagerContext<TTemplate>
    where TInfo : IItemUseInfo
    where TTemplate : IItemTemplate
{
    Task Use(
        IFieldUser user,
        ItemInventoryType type, 
        TInfo info, 
        bool exclRequest = false
    );
}
