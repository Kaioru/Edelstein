using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations;

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
