using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectPool
{
    IFieldObject? GetObject(int id);
    IEnumerable<IFieldObject> GetObjects();
    
    Task Enter(IFieldObject obj);
    Task Leave(IFieldObject obj);

    Task Dispatch(IDispatchable dispatch, IFieldObject? source = null);
}
