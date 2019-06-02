namespace Edelstein.Service.Game.Conversations.Speakers
{
    public interface ISpeech
    {
        ISpeaker Speaker { get; }
        string Text { get; }
    }
}