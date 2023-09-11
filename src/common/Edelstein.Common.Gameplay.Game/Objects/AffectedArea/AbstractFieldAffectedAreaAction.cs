using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;

namespace Edelstein.Common.Gameplay.Game.Objects.AffectedArea;

public abstract class AbstractFieldAffectedAreaAction : IFieldAffectedAreaAction
{
    public virtual Task OnEnter(IFieldObject obj) => Task.CompletedTask;
    public virtual Task OnLeave(IFieldObject obj) => Task.CompletedTask;
    public virtual Task OnTick(IFieldObject obj) => Task.CompletedTask;
}
