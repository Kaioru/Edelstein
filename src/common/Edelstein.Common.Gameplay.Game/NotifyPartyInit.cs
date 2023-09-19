using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Utilities.Pipelines;
using Foundatio.Messaging;

namespace Edelstein.Common.Gameplay.Game;

public class NotifyPartyInit : IPipelinePlug<StageStart>
{
    private readonly IMessageBus _messaging;
    private readonly IPipeline<NotifyPartyMemberUpdateChannelOrField> _notifyPartyMemberUpdateChannelOrField;
    private readonly IPipeline<NotifyPartyMemberUpdateLevelOrJob> _notifyPartyMemberUpdateLevelOrJob;
    
    public NotifyPartyInit(
        IMessageBus messaging, 
        IPipeline<NotifyPartyMemberUpdateChannelOrField> notifyPartyMemberUpdateChannelOrField, 
        IPipeline<NotifyPartyMemberUpdateLevelOrJob> notifyPartyMemberUpdateLevelOrJob
    )
    {
        _messaging = messaging;
        _notifyPartyMemberUpdateChannelOrField = notifyPartyMemberUpdateChannelOrField;
        _notifyPartyMemberUpdateLevelOrJob = notifyPartyMemberUpdateLevelOrJob;
    }
    
    public async Task Handle(IPipelineContext ctx, StageStart message)
    {
        await _messaging.SubscribeAsync<NotifyPartyMemberUpdateChannelOrField>(
            e => _notifyPartyMemberUpdateChannelOrField.Process(e)
        );
        await _messaging.SubscribeAsync<NotifyPartyMemberUpdateLevelOrJob>(
            e => _notifyPartyMemberUpdateLevelOrJob.Process(e)
        );
    }
}
