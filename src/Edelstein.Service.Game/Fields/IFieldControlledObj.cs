namespace Edelstein.Service.Game.Fields
{
    public interface IFieldControlledObj : IFieldObj
    {
        IFieldUser Controller { get; set; }
    }
}