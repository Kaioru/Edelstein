namespace Edelstein.Service.Game.Conversations.Speakers
{
    public class DefaultSpeech : IConversationSpeech
    {
        public IConversationSpeaker Speaker { get; }
        public string Text { get; }
        
        public DefaultSpeech(IConversationSpeaker speaker, string text)
        {
            Speaker = speaker;
            Text = text;
        }
    }
}