using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects;

public abstract class AbstractFieldControllable<TMovePath, TMoveAction> :
    AbstractFieldLife<TMovePath, TMoveAction>, IFieldControllable
    where TMovePath : IMovePath<TMoveAction>
    where TMoveAction : IMoveAction
{
    public IFieldController? Controller { get; private set; }

    protected AbstractFieldControllable(
        TMoveAction action, IPoint2D position, IFieldFoothold? foothold = null
    ) : base(action, position, foothold)
    {
    }

    public async Task Control(IFieldController? controller = null)
    {
        if (Controller == controller) return;

        controller?.Controlled.Remove(this);

        if (controller?.Field == Field)
            controller?.Dispatch(GetControlPacket());

        Controller = controller;

        if (controller == null) return;

        controller.Controlled.Add(this);
        await controller.Dispatch(GetControlPacket(controller));
    }

    protected abstract IPacket GetControlPacket(IFieldController? controller = null);
}
