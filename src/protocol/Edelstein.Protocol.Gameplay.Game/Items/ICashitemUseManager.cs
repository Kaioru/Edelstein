using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Items;

public interface ICashItemUseManager<out TContext, in TInfoEx, TTemplate> : 
    IPipework<TContext>
    where TContext : ICashItemUseManagerContext<TTemplate, TInfoEx>
    where TInfoEx : ICashItemUseInfoEx
    where TTemplate : IItemTemplate
{
    Task Use(
        IFieldUser user,
        ItemInventoryType type, 
        ICashItemUseInfo info, 
        TInfoEx infoEx
    );
}
