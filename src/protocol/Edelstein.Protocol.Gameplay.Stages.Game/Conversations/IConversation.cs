using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

public interface IConversation
{
    Task Start(
        IConversationContext ctx,
        IConversationSpeaker self,
        IConversationSpeaker target
    );
}
