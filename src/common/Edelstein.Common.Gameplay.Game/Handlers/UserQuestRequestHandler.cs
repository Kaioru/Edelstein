using Edelstein.Common.Gameplay.Game.Quests;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserQuestRequestHandler : AbstractFieldHandler 
{
    private readonly ILogger _logger;
        
    public UserQuestRequestHandler(ILogger<UserQuestRequestHandler> logger) => _logger = logger;
        
    public override short Operation => (short)PacketRecvOperations.UserQuestRequest;
        
    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var type = (QuestRequestType)reader.ReadByte();

        switch (type)
        {
            default:
                _logger.LogWarning("Unhandled quest request type {Type}", type);
                break;
        }
            
        return Task.CompletedTask;
    }
}
