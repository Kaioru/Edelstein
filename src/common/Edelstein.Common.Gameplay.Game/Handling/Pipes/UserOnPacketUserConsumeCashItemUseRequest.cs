using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items.Cash;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserConsumeCashItemUseRequest(
    IAdBoardCashItemUseManager adBoard
) : AbstractUserOnPacketInField<UserConsumeCashItemUseRequest>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserConsumeCashItemUseRequest> message)
    {
        var user = message.User;
        var type = ItemInventoryType.Cash;
        var info = message.Packet;
        
        switch (message.Packet.InfoEx.Value)
        {
            case StructuredAdBoardCashItemUseInfoEx adBoardInfoEx:
                await adBoard.Use(user, type, info, adBoardInfoEx);
                break;
        }
    }
}
