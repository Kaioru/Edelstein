namespace Edelstein.Service.Game.Conversations.Speakers
{
    public class Speech : ISpeech
    {
        public ISpeaker Speaker { get; }
        public string Text { get; }
        
        public Speech(ISpeaker speaker, string text)
        {
            Speaker = speaker;
            Text = text;
        }
    }
}