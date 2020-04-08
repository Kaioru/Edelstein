using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Fields.Generators
{
    public interface IFieldGenerator
    {
        ICollection<IFieldGeneratorObj> Generated { get; }

        bool Available(IField field, bool reset = false);
        Task Generate(IField field);
        Task Reset(IFieldGeneratorObj obj);
    }
}