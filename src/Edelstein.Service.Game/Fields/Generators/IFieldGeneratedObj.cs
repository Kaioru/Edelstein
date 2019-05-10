namespace Edelstein.Service.Game.Fields.Generators
{
    public interface IFieldGeneratedObj : IFieldObj
    {
        IFieldGenerator Generator { get; set; }
    }
}