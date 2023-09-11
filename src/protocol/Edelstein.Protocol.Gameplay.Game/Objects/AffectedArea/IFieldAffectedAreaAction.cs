namespace Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;

public interface IFieldAffectedAreaAction
{
    Task OnEnter(IFieldObject obj);
    Task OnLeave(IFieldObject obj);
    Task OnTick(IFieldObject obj);
}
