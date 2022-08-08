using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
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

        await message.User.Converse(
            conversation,
            c => message.NPC.GetSpeaker(c)
        );
    }
}
