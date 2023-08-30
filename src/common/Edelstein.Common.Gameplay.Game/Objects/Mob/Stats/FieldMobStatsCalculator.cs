using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

public class FieldMobStatsCalculator : IFieldMobStatsCalculator
{
    public async Task<IFieldMobStats> Calculate(IFieldMob mob)
    {
        var pad = mob.Template.PAD;
        var pdd = mob.Template.PDD;
        var pdr = mob.Template.PDR;
        var mad = mob.Template.MAD;
        var mdd = mob.Template.MDD;
        var mdr = mob.Template.MDR;
        var acc = mob.Template.ACC;
        var eva = mob.Template.EVA;
        
        pad = Math.Min(pad, 29999);
        pdd = Math.Min(pdd, 30000);
        mad = Math.Min(mad, 29999);
        mdd = Math.Min(mdd, 30000);
        acc = Math.Min(acc, 9999);
        eva = Math.Min(eva, 9999);
        
        return new FieldMobStats
        {
            Level = mob.Template.Level,
            
            PAD = pad,
            PDD = pdd,
            PDR = pdr,
            MAD = mad,
            MDD = mdd,
            MDR = mdr,
            ACC = acc,
            EVA = eva
        };
    }
}
