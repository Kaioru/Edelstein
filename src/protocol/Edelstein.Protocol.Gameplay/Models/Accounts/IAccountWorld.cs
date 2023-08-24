using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Models.Accounts;

public interface IAccountWorld : IIdentifiable<int>
{
    int AccountID { get; set; }
    int WorldID { get; set; }

    IItemLocker Locker { get; set; }
    IItemTrunk Trunk { get; set; }

    int CharacterSlotMax { get; set; }
}
