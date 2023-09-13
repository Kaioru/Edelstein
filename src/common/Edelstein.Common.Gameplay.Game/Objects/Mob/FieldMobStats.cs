using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public record FieldMobStats : IFieldMobStats
{
    public int Level { get; private set; }
    
    public int PAD { get; private set; }
    public int PDD { get; private set; }
    public int PDR { get; private set; }
    public int MAD { get; private set; }
    public int MDD { get; private set; }
    public int MDR { get; private set; }
    public int ACC { get; private set; }
    public int EVA { get; private set; }
    
    public IDictionary<Element, ElementAttribute> ElementAttributes { get; }

    public FieldMobStats()
    {
        ElementAttributes = new Dictionary<Element, ElementAttribute>();
        Reset();
    }

    public Task Apply(IFieldMob mob)
    {
        Reset();
        Level = mob.Template.Level;
        
        PAD = mob.Template.PAD;
        PDD = mob.Template.PDD;
        PDR = mob.Template.PDR;
        MAD = mob.Template.MAD;
        MDD = mob.Template.MDD;
        MDR = mob.Template.MDR;
        ACC = mob.Template.ACC;
        EVA = mob.Template.EVA;

        ElementAttributes.Clear();
        foreach (var kv in mob.Template.ElementAttributes)
            ElementAttributes[kv.Key] = kv.Value;

        PAD += mob.TemporaryStats[MobTemporaryStatType.PAD]?.Value ?? 0;
        PDR += mob.TemporaryStats[MobTemporaryStatType.PDR]?.Value ?? 0;
        MAD += mob.TemporaryStats[MobTemporaryStatType.MAD]?.Value ?? 0;
        MDR += mob.TemporaryStats[MobTemporaryStatType.MDR]?.Value ?? 0;
        ACC += mob.TemporaryStats[MobTemporaryStatType.ACC]?.Value ?? 0;
        EVA += mob.TemporaryStats[MobTemporaryStatType.EVA]?.Value ?? 0;
        
        PAD = Math.Min(PAD, 29999);
        PDD = Math.Min(PDD, 30000);
        MAD = Math.Min(MAD, 29999);
        MDD = Math.Min(MDD, 30000);
        ACC = Math.Min(ACC, 9999);
        EVA = Math.Min(EVA, 9999);
        return Task.CompletedTask;
    }
    
    public void Reset()
    {
        Level = 0;
        
        PAD = 0;
        PDD = 0;
        PDR = 0;
        MAD = 0;
        MDD = 0;
        MDR = 0;
        ACC = 0;
        EVA = 0;
        
        ElementAttributes.Clear();
    }
}
