namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

public interface IConversationSpeaker
{
    ConversationSpeakerType Type { get; }
    int TemplateID { get; }
    
    ConversationSpeakerParam Params { get; }

    byte Say(string text, bool prev = false, bool next = true);
}
