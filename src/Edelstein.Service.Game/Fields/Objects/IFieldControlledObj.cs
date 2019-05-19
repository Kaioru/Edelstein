namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldControlledObj : IFieldObj
    {
        IFieldUser Controller { get; set; }
    }
}