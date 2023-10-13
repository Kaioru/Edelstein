using Edelstein.Common.Gameplay.Game.Dialogues;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserShopRequestHandler : AbstractFieldHandler
{
    private readonly ILogger _logger;
    
    public UserShopRequestHandler(ILogger<UserShopRequestHandler> logger) => _logger = logger;
    
    public override short Operation => (short)PacketRecvOperations.UserShopRequest;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        if (user.ActiveDialogue is not IDialogueNPCShop shop) return;
        
        var type = (NPCShopRequestOperations)reader.ReadByte();

        switch (type)
        {
            case NPCShopRequestOperations.Buy:
            {
                var position = reader.ReadShort();
                var templateID = reader.ReadInt();
                var count = reader.ReadShort();

                await user.StageUser.Context.Pipelines.FieldOnPacketUserShopBuyRequest.Process(new FieldOnPacketUserShopBuyRequest(
                    user,
                    shop,
                    position,
                    templateID,
                    count
                ));
                break;
            }
            case NPCShopRequestOperations.Sell:
            {
                var slot = reader.ReadShort();
                var templateID = reader.ReadInt();
                var count = reader.ReadShort();
                
                await user.StageUser.Context.Pipelines.FieldOnPacketUserShopSellRequest.Process(new FieldOnPacketUserShopSellRequest(
                    user,
                    shop,
                    slot,
                    templateID,
                    count
                ));
                break;
            }
            case NPCShopRequestOperations.Recharge:
            {
                var slot = reader.ReadShort();
                
                await user.StageUser.Context.Pipelines.FieldOnPacketUserShopRechargeRequest.Process(new FieldOnPacketUserShopRechargeRequest(
                    user,
                    shop,
                    slot
                ));
                break;
            }
            case NPCShopRequestOperations.Close:
                await user.StageUser.Context.Pipelines.FieldOnPacketUserShopCloseRequest.Process(new FieldOnPacketUserShopCloseRequest(
                    user,
                    shop
                ));
                break;
            default:
                _logger.LogWarning("Unhandled shop request type {Type}", type);
                break;
        }
    }
}
