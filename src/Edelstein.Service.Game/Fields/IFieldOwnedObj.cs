namespace Edelstein.Service.Game.Fields
{
    public interface IFieldOwnedObj : IFieldObj
    {
        IFieldUser Owner { get; }
    }
}