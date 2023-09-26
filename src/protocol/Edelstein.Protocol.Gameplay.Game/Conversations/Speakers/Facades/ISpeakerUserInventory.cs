namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;

public interface ISpeakerUserInventory
{
    void Add(int templateID, short count = 1);
    void Remove(int templateID, short count = 1);
}
