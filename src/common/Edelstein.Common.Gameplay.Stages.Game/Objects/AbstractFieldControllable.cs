using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldControllable<TMovePath, TMoveAction> :
    AbstractFieldLife<TMovePath, TMoveAction>, IFieldControllable
    where TMovePath : IMovePath<TMoveAction>
    where TMoveAction : IMoveAction
{
    protected AbstractFieldControllable(
        TMoveAction action, IPoint2D position, IFieldFoothold? foothold = null
    ) : base(action, position, foothold)
    {
    }

    public IFieldController? Controller { get; private set; }

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
