namespace Edelstein.Service.Game.Conversations.Speakers
{
    public interface IConversationSpeech
    {
        IConversationSpeaker Speaker { get; }
        string Text { get; }
    }
}