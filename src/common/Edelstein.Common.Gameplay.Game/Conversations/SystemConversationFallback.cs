using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class SystemConversationFallback<TSelf, TTarget>(
    string script,
    IFieldUser user
) : IConversation<TSelf, TTarget>
    where TSelf : IConversationSpeaker 
    where TTarget : IConversationSpeaker
{
    public Task Start(IConversationContext ctx, TSelf self, TTarget target)
        => user.Message($"The scripted conversation '{script}' is not available.");
}
