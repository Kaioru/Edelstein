using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class SystemConversation(
    Action<IConversationSpeaker, IConversationSpeaker> action    
) : IConversation<IConversationSpeaker, IConversationSpeaker>
{

    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target)
    {
        action.Invoke(self, target);
        return Task.CompletedTask;
    }
}
