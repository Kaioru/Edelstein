using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class TestConversation : IConversation<IConversationSpeaker, IConversationSpeaker>
{
    public Task Start(IConversationContext ctx, IConversationSpeaker self, IConversationSpeaker target)
    {
        self.Say("hey you!");
        self.Say("yeah u!");
        self.Say("uwu nya!");
        return Task.CompletedTask;
    }
}
