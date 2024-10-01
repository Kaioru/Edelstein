using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Game.Items.Cash;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Cash;

public class AdBoardCashItemUseManager(
    ITemplateManager<IItemTemplate> templates
) : AbstractCashItemUseManager<IAdBoardCashItemUseManagerContext, StructuredAdBoardCashItemUseInfoEx, IItemTemplate>(templates, true), 
    IAdBoardCashItemUseManager
{
    protected override IAdBoardCashItemUseManagerContext Create(
        IFieldUser user,
        ItemSlotBase item, 
        IItemTemplate template,
        ICashItemUseInfo info, 
        StructuredAdBoardCashItemUseInfoEx infoEx
    )
        => new AdBoardCashItemUseManagerContext(user, item, template, info, infoEx);
    
    protected override Task Handle(IAdBoardCashItemUseManagerContext context, IFieldUser user)
    {
        // TODO: Set AdBoard
        return Task.CompletedTask;
    }
}
