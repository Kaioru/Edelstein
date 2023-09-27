namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

public interface IConversationSpeech
{
    IConversationSpeaker Speaker { get; }
    string Message { get; }
}
