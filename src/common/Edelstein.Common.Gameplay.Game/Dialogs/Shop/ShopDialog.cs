using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Shop;

public class ShopDialog(
    IShopTemplate template
) : IShopDialog
{
    public Task OnOpen(IFieldUser user) => Task.CompletedTask;
    public Task OnClose(IFieldUser user) => Task.CompletedTask;
    
    public Task Buy(IFieldUser user, UserShopRequestInfoBuy info) => throw new System.NotImplementedException();
    
    public Task Sell(IFieldUser user, UserShopRequestInfoSell info) => throw new System.NotImplementedException();
    
    public Task Recharge(IFieldUser user, UserShopRequestInfoRecharge info) => throw new System.NotImplementedException();
    
    public Task Close(IFieldUser user) => user.EndDialog();
}
