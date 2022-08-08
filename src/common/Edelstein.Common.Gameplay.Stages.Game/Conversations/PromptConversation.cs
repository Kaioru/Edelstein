using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations;

public class PromptConversation : IConversation
{
    private readonly Action<IConversationSpeaker, IConversationSpeaker> _action;

    public PromptConversation(Action<IConversationSpeaker, IConversationSpeaker> action) => _action = action;

    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target)
    {
        _action.Invoke(self, target);
        return Task.CompletedTask;
    }
}
