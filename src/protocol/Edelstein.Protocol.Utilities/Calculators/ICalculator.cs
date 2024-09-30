using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Utilities.Calculators;

public interface ICalculator<in TInput, out TContext, TOutput> : IPipework<TContext>
{
    Task<TOutput> Calculate(TInput input);
}
