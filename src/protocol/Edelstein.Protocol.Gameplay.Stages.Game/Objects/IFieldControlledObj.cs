namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects
{
    public interface IFieldControlledObj : IFieldObj
    {
        IFieldObjUser Controller { get; set; }
    }
}
