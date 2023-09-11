using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat;

public static class SkillContextExtensions
{
    public static void ResetTemporaryStatComboCounter(this ISkillContext context, int comboCounter = 0) 
        => context.ResetTemporaryStatExisting(TemporaryStatType.ComboCounter, comboCounter + 1);
    
    public static void ResetTemporaryStatAuras(this ISkillContext context)
    {
        context.ResetTemporaryStatByType(TemporaryStatType.DarkAura);
        context.ResetTemporaryStatByType(TemporaryStatType.BlueAura);
        context.ResetTemporaryStatByType(TemporaryStatType.YellowAura);
    }

    public static void ResetTemporaryStatNegative(this ISkillContext context)
    {
        context.ResetTemporaryStatByType(TemporaryStatType.Stun);
        context.ResetTemporaryStatByType(TemporaryStatType.Poison);
        context.ResetTemporaryStatByType(TemporaryStatType.Seal);
        context.ResetTemporaryStatByType(TemporaryStatType.Darkness);
        context.ResetTemporaryStatByType(TemporaryStatType.Thaw);
        context.ResetTemporaryStatByType(TemporaryStatType.Weakness);
        context.ResetTemporaryStatByType(TemporaryStatType.Curse);
        context.ResetTemporaryStatByType(TemporaryStatType.Slow);
        context.ResetTemporaryStatByType(TemporaryStatType.Blind);
    }
}
