using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeech : IConversationSpeech
{
    public ConversationSpeech(IConversationSpeaker speaker, string message)
    {
        Speaker = speaker;
        Message = message;
    }
    
    public IConversationSpeaker Speaker { get; }
    public string Message { get; }
}
