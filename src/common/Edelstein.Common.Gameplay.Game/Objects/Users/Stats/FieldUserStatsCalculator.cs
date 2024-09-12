using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Combat.Stats;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats;

public class FieldUserStatsCalculator : 
    Pipeline<IFieldUserStatsCalculatorContext>,
    IFieldUserStatsCalculator
{
    public FieldUserStatsCalculator(
        IEnumerable<IFieldUserStatsCalculatorEntry> entries
    ) 
    {
        foreach (var entry in entries) 
            Add(entry.Priority, entry);
    }
    
    public async Task<IFieldUserStats> Calculate(IFieldUser input)
    {
        var context = new FieldUserStatsCalculatorContext(input);
        var character = input.Character;
        
        await Process(context);

        var totalSTR = context.STR.Apply(character.STR);
        var totalDEX = context.DEX.Apply(character.DEX);
        var totalINT = context.INT.Apply(character.INT);
        var totalLUK = context.LUK.Apply(character.LUK);

        context.PDD.IncFlat += (int)(totalSTR * 1.2 + totalLUK * 0.5 + totalDEX * 0.5 + totalINT * 0.4);
        context.MDD.IncFlat += (int)(totalINT * 1.2 + totalDEX * 0.5 + totalLUK * 0.5 + totalSTR * 0.4);
        
        var pacc = new StatModifier(Max: 9999);
        var macc = new StatModifier(Max: 9999);
        var peva = new StatModifier(Max: 9999);
        var meva = new StatModifier(Max: 9999);

        pacc.IncRate = context.ACC.IncRate;
        macc.IncRate = context.ACC.IncRate;
        peva.IncRate = context.EVA.IncRate;
        meva.IncRate = context.EVA.IncRate;
        
        return new FieldUserStats
        {
            STR = totalSTR,
            DEX = totalDEX,
            INT = totalINT,
            LUK = totalLUK,
            
            MaxHP = context.MaxHP.Apply(character.MaxHP),
            MaxMP = context.MaxMP.Apply(character.MaxMP),
            
            PAD = context.PAD.Apply(0),
            PDD = context.PDD.Apply(0),
            MAD = context.MAD.Apply(0),
            MDD = context.MDD.Apply(0),
            
            PACC = pacc.Apply((int)(totalDEX * 1.2 + totalLUK) + context.ACC.IncBase),
            MACC = pacc.Apply((int)(totalLUK * 1.2 + totalINT) + context.ACC.IncBase),
            PEVA = pacc.Apply(totalLUK * 2 + totalDEX + context.EVA.IncBase),
            MEVA = pacc.Apply(totalLUK * 2 + totalINT + context.EVA.IncBase),
            
            Craft = context.Craft.Apply(0),
            Speed = context.Speed.Apply(100),
            Jump = context.Jump.Apply(100)
        };
    }
}
