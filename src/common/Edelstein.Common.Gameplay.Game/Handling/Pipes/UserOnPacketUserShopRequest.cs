using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserShopRequest : AbstractUserOnPacketInField<UserShopRequest>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserShopRequest> message)
    {
        if (message.User.ActiveDialog is not IShopDialog shop) return;

        switch (message.Packet.Info)
        {
            case UserShopRequestInfoBuy buy:
                await shop.Buy(message.User, buy);
                break;
            case UserShopRequestInfoSell sell:
                await shop.Sell(message.User, sell);
                break;
            case UserShopRequestInfoRecharge recharge:
                await shop.Recharge(message.User, recharge);
                break;
            default:
                await shop.Close(message.User);
                break;
        }
    }
}
