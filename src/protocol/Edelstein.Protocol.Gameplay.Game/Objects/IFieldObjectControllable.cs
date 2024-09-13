using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectControllable : IFieldObject
{
    IFieldObjectController? Controller { get; }

    Task Control(IFieldObjectController? controller);
    
    IDispatchable GetDispatchChangeController(IFieldObjectController? controller = null);
}
