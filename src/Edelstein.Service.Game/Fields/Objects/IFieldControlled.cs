namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldControlled : IFieldObj
    {
        IFieldUser Controller { get; set; }
    }
}