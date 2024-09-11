using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Combat.Stats;

public interface IStatsCalculator<in TInput, out TContext, TOutput> : 
    IPipework<TContext>
    where TOutput : IStats
{
    Task<TOutput> Calculate(TInput input);
}
