using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Shop;

public interface IShopDialog : IDialog
{
    Task Buy(IFieldUser user, UserShopRequestInfoBuy info);
    Task Sell(IFieldUser user, UserShopRequestInfoSell info);
    Task Recharge(IFieldUser user, UserShopRequestInfoRecharge info);
}
