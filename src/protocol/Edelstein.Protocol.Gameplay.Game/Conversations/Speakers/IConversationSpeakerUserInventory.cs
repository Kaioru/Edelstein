namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

public interface IConversationSpeakerUserInventory
{
    void Add(int templateID, short count = 1);
    void Remove(int templateID, short count = 1);
}
