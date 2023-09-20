using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Foundatio.Messaging;

namespace Edelstein.Common.Gameplay.Game;

public class NotifyFriendInit : IPipelinePlug<StageStart>
{
    private readonly IMessageBus _messaging;
    private readonly IPipeline<NotifyFriendUpdateChannel> _notifyFriendUpdateChannel;
    
    public NotifyFriendInit(IMessageBus messaging, IPipeline<NotifyFriendUpdateChannel> notifyFriendUpdateChannel)
    {
        _messaging = messaging;
        _notifyFriendUpdateChannel = notifyFriendUpdateChannel;
    }
    
    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        await _messaging.SubscribeAsync<NotifyFriendUpdateChannel>(
            e => _notifyFriendUpdateChannel.Process(e)
        );
    }
}
