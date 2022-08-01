using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Inventories;

namespace Edelstein.Common.Gameplay.Accounts;

public record AccountWorld : IAccountWorld
{
    public int ID { get; set; }
    public int AccountID { get; set; }
    public int WorldID { get; set; }

    public IItemLocker Locker { get; set; }
    public IItemTrunk Trunk { get; set; }

    public int CharacterSlotMax { get; set; } = 3;
}
