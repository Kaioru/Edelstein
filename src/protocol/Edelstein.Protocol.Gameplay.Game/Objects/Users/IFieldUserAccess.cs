using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Accessors;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public interface IFieldUserAccess
{
    Task<T> Inventory<T>(Func<IAccessInventory, Task<T>> action);
}
