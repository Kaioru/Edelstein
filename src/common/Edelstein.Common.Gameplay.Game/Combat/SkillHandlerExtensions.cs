using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Game.Combat;

public static class SkillHandlerExtensions
{
    public static async Task HandleAttackComboCounter(this ISkillHandler handler, ISkillContext context, IFieldUser user)
    {
        var comboCounterStat = user.Character.TemporaryStats[TemporaryStatType.ComboCounter];

        if (comboCounterStat != null && context.Skill?.ID is
            Skill.CrusaderPanic or
            Skill.CrusaderComa or
            Skill.SoulmasterPanicSword or
            Skill.SoulmasterComaSword)
            context.ResetComboCounter();
        else if (comboCounterStat != null && context.IsHitMob)
        {
            var comboCounterSkillID = JobConstants.GetJobRace(user.Character.Job) == 0
                ? Skill.CrusaderComboAttack
                : Skill.SoulmasterComboAttack;
            var comboCounterSkill = await user.StageUser.Context.Templates.Skill.Retrieve(comboCounterSkillID);
            var comboCounterLevel = comboCounterSkill?[user.Stats.SkillLevels[comboCounterStat.Reason]];
            var comboCounter = comboCounterStat.Value - 1;
            var comboMax = comboCounterLevel?.X ?? 0;

            var advComboCounterSkillID = JobConstants.GetJobRace(user.Character.Job) == 0
                ? Skill.HeroAdvancedCombo
                : Skill.SoulmasterAdvancedCombo;
            var advComboCounterSkill = await user.StageUser.Context.Templates.Skill.Retrieve(advComboCounterSkillID);
            var advComboCounterLevel = advComboCounterSkill?[user.Stats.SkillLevels[advComboCounterSkillID]];
            var comboDoubleChance = advComboCounterLevel?.Prop ?? 0;
            
            comboMax = advComboCounterLevel?.X ?? comboMax;
            
            if (comboCounter < comboMax)
                await user.ModifyTemporaryStats(s => s.Set(
                    TemporaryStatType.ComboCounter,
                    Math.Min(comboMax + 1, comboCounterStat.Value + (context.Random.Next(0, 100) <= comboDoubleChance ? 2 : 1)),
                    comboCounterStat.Reason,
                    comboCounterStat.DateExpire
                ));
        }
    }
    
    public static Task HandleSkillUseBasic(this ISkillHandler handler, ISkillContext context, IFieldUser user)
    {
        if (context.SkillLevel?.PAD > 0)
            context.AddTemporaryStat(TemporaryStatType.PAD, context.SkillLevel.PAD);
        if (context.SkillLevel?.PDD > 0)
            context.AddTemporaryStat(TemporaryStatType.PDD, context.SkillLevel.PDD);
        if (context.SkillLevel?.MAD > 0)
            context.AddTemporaryStat(TemporaryStatType.MAD, context.SkillLevel.MAD);
        if (context.SkillLevel?.MDD > 0)
            context.AddTemporaryStat(TemporaryStatType.MDD, context.SkillLevel.MDD);
        
        if (context.SkillLevel?.EPAD > 0)
            context.AddTemporaryStat(TemporaryStatType.EPAD, context.SkillLevel.EPAD);
        if (context.SkillLevel?.EPDD > 0)
            context.AddTemporaryStat(TemporaryStatType.EPDD, context.SkillLevel.EPDD);
        if (context.SkillLevel?.EMDD > 0)
            context.AddTemporaryStat(TemporaryStatType.EMDD, context.SkillLevel.EMDD);
        
        if (context.SkillLevel?.ACC > 0)
            context.AddTemporaryStat(TemporaryStatType.ACC, context.SkillLevel.ACC);
        if (context.SkillLevel?.EVA > 0)
            context.AddTemporaryStat(TemporaryStatType.EVA, context.SkillLevel.EVA);
        
        if (context.SkillLevel?.Speed > 0)
            context.AddTemporaryStat(TemporaryStatType.Speed, context.SkillLevel.Speed);
        if (context.SkillLevel?.Jump > 0)
            context.AddTemporaryStat(TemporaryStatType.Jump, context.SkillLevel.Jump);
        return Task.CompletedTask;
    }
}
