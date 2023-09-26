namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;

public interface ISpeakerUserInventory
{
    void Add(int templateID, short count = 1);
    void Remove(int templateID, short count = 1);

    int CountItem(int templateID);
    bool HasItem(int templateID, short count = 1);
    bool HasEquipped(int templateID);
    bool HasSlotFor(int templateID, short count = 1);
    bool HasSlotFor(IDictionary<int, short> templates);
}
