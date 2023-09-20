using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;
using Foundatio.Messaging;

namespace Edelstein.Common.Gameplay.Game;

public class NotifyFriendInit : IPipelinePlug<StageStart>
{
    private readonly IMessageBus _messaging;
    private readonly IPipeline<NotifyFriendInvited> _notifyFriendInvited;
    private readonly IPipeline<NotifyFriendUpdateChannel> _notifyFriendUpdateChannel;
    private readonly IPipeline<NotifyFriendUpdateList> _notifyFriendUpdateList;

    public NotifyFriendInit(IMessageBus messaging, IPipeline<NotifyFriendInvited> notifyFriendInvited, IPipeline<NotifyFriendUpdateChannel> notifyFriendUpdateChannel, IPipeline<NotifyFriendUpdateList> notifyFriendUpdateList)
    {
        _messaging = messaging;
        _notifyFriendInvited = notifyFriendInvited;
        _notifyFriendUpdateChannel = notifyFriendUpdateChannel;
        _notifyFriendUpdateList = notifyFriendUpdateList;
    }
    
    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        await _messaging.SubscribeAsync<NotifyFriendInvited>(
            e => _notifyFriendInvited.Process(e)
        );
        await _messaging.SubscribeAsync<NotifyFriendUpdateChannel>(
            e => _notifyFriendUpdateChannel.Process(e)
        );
        await _messaging.SubscribeAsync<NotifyFriendUpdateList>(
            e => _notifyFriendUpdateList.Process(e)
        );
    }
}
