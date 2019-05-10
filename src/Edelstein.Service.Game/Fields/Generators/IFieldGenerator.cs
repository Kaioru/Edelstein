using System;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Fields.Generators
{
    public interface IFieldGenerator
    {
        TimeSpan RegenInterval { get; }
        DateTime RegenAfter { get; }

        IFieldGeneratedObj Generated { get; }

        Task Reset();
        Task Generate(IField field);
    }
}