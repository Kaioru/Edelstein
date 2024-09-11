using System.Threading.Tasks;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats;

public class FieldUserStatsCalculator : 
    Pipeline<IFieldUserStatsCalculatorContext>,
    IFieldUserStatsCalculator
{
    public async Task<IFieldUserStats> Calculate(IFieldUser input)
    {
        var context = new FieldUserStatsCalculatorContext(input);
        var character = input.Character;
        
        await Process(context);

        return new FieldUserStats
        {
            STR = context.STR.Apply(character.STR),
            DEX = context.DEX.Apply(character.DEX),
            INT = context.INT.Apply(character.INT),
            LUK = context.LUK.Apply(character.LUK),
            
            MaxHP = context.MaxHP.Apply(character.MaxHP),
            MaxMP = context.MaxMP.Apply(character.MaxMP),
            
            PAD = context.PAD.Apply(0),
            PDD = context.PDD.Apply(0),
            MAD = context.MAD.Apply(0),
            MDD = context.MDD.Apply(0),
            ACC = context.ACC.Apply(0),
            EVA = context.EVA.Apply(0),
            
            Craft = context.Craft.Apply(0),
            Speed = context.Speed.Apply(100),
            Jump = context.Jump.Apply(100)
        };
    }
}
