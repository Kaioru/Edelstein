using Edelstein.Common.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserSelectNPCPlug : IPipelinePlug<IUserSelectNPC>
{
    private readonly IConversationManager _manager;

    public UserSelectNPCPlug(IConversationManager manager) => _manager = manager;

    public async Task Handle(IPipelineContext ctx, IUserSelectNPC message)
    {
        var script = message.NPC.Template.Scripts.FirstOrDefault()?.Script;
        if (script == null) return;
        var conversation = await _manager.Create(script);

        _ = message.User.Converse(
            conversation,
            c => new ConversationSpeaker(c, message.NPC.Template.ID),
            c => new ConversationSpeaker(c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
        );
    }
}
