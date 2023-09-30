using Edelstein.Common.Gameplay.Game.Dialogues;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserShopRequestHandler : AbstractFieldHandler
{
    private readonly ILogger _logger;
    
    public UserShopRequestHandler(ILogger<UserShopRequestHandler> logger) => _logger = logger;
    
    public override short Operation => (short)PacketRecvOperations.UserShopRequest;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        if (user.ActiveDialogue is not IDialogueNPCShop) return;
        
        var type = (NPCShopRequestOperations)reader.ReadByte();

        switch (type)
        {
            case NPCShopRequestOperations.Close:
                await user.EndDialogue();
                await user.ModifyStats(exclRequest: true);
                break;
            default:
                _logger.LogWarning("Unhandled shop request type {Type}", type);
                break;
        }
    }
}
