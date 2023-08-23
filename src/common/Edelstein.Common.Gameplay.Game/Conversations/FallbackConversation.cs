using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class FallbackConversation : IConversation
{
    private readonly string _name;

    public FallbackConversation(string name) => _name = name;

    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target)
    {
        self.Say($"The scripted conversation '{_name}' is not available");
        return Task.CompletedTask;
    }
}
