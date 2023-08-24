using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface IConversation
{
    Task Start(
        IConversationContext ctx,
        IConversationSpeaker self,
        IConversationSpeaker target
    );
}
