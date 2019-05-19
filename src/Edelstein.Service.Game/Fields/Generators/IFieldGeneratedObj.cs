using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields.Generators
{
    public interface IFieldGeneratedObj : IFieldObj
    {
        IFieldGenerator Generator { get; set; }
    }
}