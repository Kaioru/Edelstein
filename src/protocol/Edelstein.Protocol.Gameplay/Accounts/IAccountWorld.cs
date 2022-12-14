using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Protocol.Gameplay.Accounts;

public interface IAccountWorld : IIdentifiable<int>
{
    int AccountID { get; set; }
    int WorldID { get; set; }

    IItemLocker Locker { get; set; }
    IItemTrunk Trunk { get; set; }

    int CharacterSlotMax { get; set; }
}
