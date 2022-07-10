namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.MessageBox
{
    public interface IFieldObjMessageBox : IFieldObj
    {
        int ItemID { get; }
        string Hope { get; }
        string Name { get; }
    }
}
