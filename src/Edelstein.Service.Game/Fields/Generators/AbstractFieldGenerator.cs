using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Fields.Generators
{
    public abstract class AbstractFieldGenerator : IFieldGenerator
    {
        public ICollection<IFieldGeneratorObj> Generated { get; }

        protected AbstractFieldGenerator()
            => Generated = new List<IFieldGeneratorObj>();

        public abstract bool Available(IField field, bool reset = false);
        public abstract Task Generate(IField field);
        public abstract Task Reset(IFieldGeneratorObj obj);
    }
}