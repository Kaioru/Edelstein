using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldSplit : IFieldObjectPool
{
    int Row { get; }
    int Col { get; }

    IEnumerable<IFieldSplitObserver> GetObservers();
    
    Task MigrateIn(IFieldObject obj);
    Task MigrateOut(IFieldObject obj);

    Task Observe(IFieldSplitObserver observer);
    Task Unobserve(IFieldSplitObserver observer, bool isLeaveField = false);
}
