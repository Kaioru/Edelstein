using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldObjectPool : IFieldObjectPool
{
    public abstract IFieldObject? GetObject(int id);
    public abstract IEnumerable<IFieldObject> GetObjects();

    public abstract Task Enter(IFieldObject obj);
    public abstract Task Leave(IFieldObject obj);
    
    public Task Dispatch(IDispatchable dispatch, IFieldObject? source = null)
        => Task.WhenAll(GetObjects()
            .OfType<IFieldUser>()
            .Where(o => o != source)
            .Select(o => o.Dispatch(dispatch)));
}
