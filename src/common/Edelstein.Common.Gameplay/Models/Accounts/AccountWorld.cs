using Edelstein.Common.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Common.Gameplay.Models.Accounts;

public record AccountWorld : IAccountWorld
{
    public int ID { get; set; }
    public int AccountID { get; set; }
    public int WorldID { get; set; }

    public IItemLocker Locker { get; set; } = new ItemLocker();
    public IItemTrunk Trunk { get; set; } = new ItemTrunk();

    public int CharacterSlotMax { get; set; } = 3;
}
