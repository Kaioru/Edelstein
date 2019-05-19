namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldOwnedObj : IFieldObj
    {
        IFieldUser Owner { get; }
    }
}