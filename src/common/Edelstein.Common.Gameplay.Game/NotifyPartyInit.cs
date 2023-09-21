using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Utilities.Pipelines;
using Foundatio.Messaging;

namespace Edelstein.Common.Gameplay.Game;

public class NotifyPartyInit : IPipelinePlug<StageStart>
{
    private readonly IMessageBus _messaging;
    private readonly IPipeline<NotifyPartyCreated> _notifyPartyCreated;
    private readonly IPipeline<NotifyPartyDisbanded> _notifyPartyDisbanded;
    private readonly IPipeline<NotifyPartyMemberInvited> _notifyPartyMemberInvited;
    private readonly IPipeline<NotifyPartyMemberUpdateChannelOrField> _notifyPartyMemberUpdateChannelOrField;
    private readonly IPipeline<NotifyPartyMemberUpdateLevelOrJob> _notifyPartyMemberUpdateLevelOrJob;
    
    public NotifyPartyInit(IMessageBus messaging, IPipeline<NotifyPartyCreated> notifyPartyCreated, IPipeline<NotifyPartyDisbanded> notifyPartyDisbanded, IPipeline<NotifyPartyMemberInvited> notifyPartyMemberInvited, IPipeline<NotifyPartyMemberUpdateChannelOrField> notifyPartyMemberUpdateChannelOrField, IPipeline<NotifyPartyMemberUpdateLevelOrJob> notifyPartyMemberUpdateLevelOrJob)
    {
        _messaging = messaging;
        _notifyPartyCreated = notifyPartyCreated;
        _notifyPartyDisbanded = notifyPartyDisbanded;
        _notifyPartyMemberInvited = notifyPartyMemberInvited;
        _notifyPartyMemberUpdateChannelOrField = notifyPartyMemberUpdateChannelOrField;
        _notifyPartyMemberUpdateLevelOrJob = notifyPartyMemberUpdateLevelOrJob;
    }
    
    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        await _messaging.SubscribeAsync<NotifyPartyCreated>(
            e => _notifyPartyCreated.Process(e)
        );
        await _messaging.SubscribeAsync<NotifyPartyDisbanded>(
            e => _notifyPartyDisbanded.Process(e)
        );
        await _messaging.SubscribeAsync<NotifyPartyMemberInvited>(
            e => _notifyPartyMemberInvited.Process(e)
        );
        await _messaging.SubscribeAsync<NotifyPartyMemberUpdateChannelOrField>(
            e => _notifyPartyMemberUpdateChannelOrField.Process(e)
        );
        await _messaging.SubscribeAsync<NotifyPartyMemberUpdateLevelOrJob>(
            e => _notifyPartyMemberUpdateLevelOrJob.Process(e)
        );
    }
}
