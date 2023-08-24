using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldSplit : IFieldObjectPool
{
    int Row { get; }
    int Col { get; }

    IReadOnlyCollection<IFieldSplitObserver> Observers { get; }

    Task Enter(IFieldObject obj, Func<IPacket>? getEnterPacket = null, Func<IPacket>? getLeavePacket = null);
    Task Leave(IFieldObject obj, Func<IPacket>? getLeavePacket = null);

    Task MigrateIn(IFieldObject obj);
    Task MigrateOut(IFieldObject obj);

    Task Observe(IFieldSplitObserver observer);
    Task Unobserve(IFieldSplitObserver observer);
}
