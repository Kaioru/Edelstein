namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.MessageBox;

public interface IFieldMessageBox : IFieldObject
{
    int ItemID { get; }
    string Hope { get; }
    string Name { get; }
}
