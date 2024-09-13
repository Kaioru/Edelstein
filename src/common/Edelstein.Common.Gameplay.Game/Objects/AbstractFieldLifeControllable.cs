using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldLifeControllable<TMovePath, TMoveAction>(
    IPoint2D position, 
    IFieldFoothold? foothold, 
    TMoveAction action
) : AbstractFieldLife<TMovePath, TMoveAction>(position, foothold, action), IFieldObjectControllable
    where TMovePath : IMovePath<TMoveAction>
    where TMoveAction : IMoveAction
{
    public IFieldObjectController? Controller { get; private set; }
    
    public async Task Control(IFieldObjectController? controller)
    {
        if (Controller == controller) return;

        controller?.Controlling.Remove(this);

        if (controller?.Field == Field)
            controller?.Dispatch(GetDispatchChangeController());

        Controller = controller;

        if (controller == null) return;

        controller.Controlling.Add(this);
        
        await controller.Dispatch(GetDispatchChangeController(controller));
    }

    public abstract IDispatchable GetDispatchChangeController(IFieldObjectController? controller = null);
}
