using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldSplit : IFieldObjectPool
{
    int Row { get; }
    int Col { get; }

    IEnumerable<IFieldUser> GetObservers();
    
    Task MigrateIn(IFieldObject obj);
    Task MigrateOut(IFieldObject obj);

    Task Observe(IFieldUser user);
    Task Unobserve(IFieldUser user, bool isLeaveField = false);
}
