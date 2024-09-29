using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats.Calculations;

public class FieldUserStatsCalculatorTemporaryStat : 
    IFieldUserStatsCalculatorEntry
{
    public int Priority => FieldUserStatsCalculatorSteps.TemporaryStat;
    
    public Task Handle(IPipelineContext ctx, IFieldUserStatsCalculatorContext stats)
    {
        var temporaryStats = stats.User.Character.TemporaryStats;

        stats.PAD.IncBase += temporaryStats[TemporaryStatType.PAD]?.Option ?? 0;
        stats.PDD.IncBase += temporaryStats[TemporaryStatType.PDD]?.Option ?? 0;
        stats.MAD.IncBase += temporaryStats[TemporaryStatType.MAD]?.Option ?? 0;
        stats.MDD.IncBase += temporaryStats[TemporaryStatType.MDD]?.Option ?? 0;
        
        stats.PAD.IncBase += temporaryStats[TemporaryStatType.EPAD]?.Option ?? 0;
        stats.PDD.IncBase += temporaryStats[TemporaryStatType.EPDD]?.Option ?? 0;
        stats.MDD.IncBase += temporaryStats[TemporaryStatType.EMDD]?.Option ?? 0;
        
        stats.ACC.IncBase += temporaryStats[TemporaryStatType.ACC]?.Option ?? 0;
        stats.EVA.IncBase += temporaryStats[TemporaryStatType.EVA]?.Option ?? 0;
        
        stats.Craft.IncBase += temporaryStats[TemporaryStatType.Craft]?.Option ?? 0;
        stats.Speed.IncBase += temporaryStats[TemporaryStatType.Speed]?.Option ?? 0;
        stats.Jump.IncBase += temporaryStats[TemporaryStatType.Jump]?.Option ?? 0;
        
        stats.MaxHP.IncRate += temporaryStats[TemporaryStatType.MaxHP]?.Option ?? 0;
        stats.MaxMP.IncRate += temporaryStats[TemporaryStatType.MaxMP]?.Option ?? 0;
        
        stats.STR.IncRate += temporaryStats[TemporaryStatType.BasicStatUp]?.Option ?? 0;
        stats.DEX.IncRate += temporaryStats[TemporaryStatType.BasicStatUp]?.Option ?? 0;
        stats.INT.IncRate += temporaryStats[TemporaryStatType.BasicStatUp]?.Option ?? 0;
        stats.LUK.IncRate += temporaryStats[TemporaryStatType.BasicStatUp]?.Option ?? 0;
        
        return Task.CompletedTask;
    }
}
