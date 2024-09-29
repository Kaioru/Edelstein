using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Combat.Stats;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
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
        var character = input.Character;
        var weapon = (character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) ?? false
            ? result1.TemplateID
            : 0).GetWeaponType();
        var weaponSub = (character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Shield, out var result2) ?? false
            ? result2.TemplateID
            : 0).GetWeaponType();
        var context = new FieldUserStatsCalculatorContext(input, weapon, weaponSub);

        await Process(context);

        var totalSTR = context.STR.Apply(character.STR);
        var totalDEX = context.DEX.Apply(character.DEX);
        var totalINT = context.INT.Apply(character.INT);
        var totalLUK = context.LUK.Apply(character.LUK);

        context.PDD.IncFlat += (int)(totalSTR * 1.2 + totalLUK * 0.5 + totalDEX * 0.5 + totalINT * 0.4);
        context.MDD.IncFlat += (int)(totalINT * 1.2 + totalDEX * 0.5 + totalLUK * 0.5 + totalSTR * 0.4);

        var totalPAD = context.PAD.Apply(0);
        var totalMAD = context.MAD.Apply(0);
        var totalMastery = context.Mastery.Apply(0);

        var (damageMin, damageMax) = CalculateDamage(
            input.Character.Job,
            weapon,
            totalMastery,
            totalSTR,
            totalDEX,
            totalINT,
            totalLUK,
            totalPAD,
            totalMAD
        );
        var totalDamageMin = context.DamageMin.Apply(damageMin);
        var totalDamageMax = context.DamageMax.Apply(damageMax);

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

            PAD = totalPAD,
            PDD = context.PDD.Apply(0),
            MAD = totalMAD,
            MDD = context.MDD.Apply(0),

            PACC = pacc.Apply((int)(totalDEX * 1.2 + totalLUK) + context.ACC.IncBase),
            MACC = macc.Apply((int)(totalLUK * 1.2 + totalINT) + context.ACC.IncBase),
            PEVA = peva.Apply(totalLUK * 2 + totalDEX + context.EVA.IncBase),
            MEVA = meva.Apply(totalLUK * 2 + totalINT + context.EVA.IncBase),

            Craft = context.Craft.Apply(character.DEX + character.INT + character.LUK),
            Speed = context.Speed.Apply(100),
            Jump = context.Jump.Apply(100),
            
            Mastery = totalMastery,
            
            DamageMin = Math.Min(totalDamageMin, totalDamageMax),
            DamageMax = Math.Max(totalDamageMin, totalDamageMax)
        };
    }

    private Tuple<int, int> CalculateDamage(
        int job,
        WeaponType weapon,
        int totalMastery,
        int totalSTR,
        int totalDEX,
        int totalINT,
        int totalLUK,
        int totalPAD,
        int totalMAD
    )
    {
        var stat1 = 0;
        var stat2 = 0;
        var stat3 = 0;
        var attack = totalPAD;
        var multiplier = 1.0;

        if (job.GetJobLevel() == 0)
        {
            stat1 = totalSTR;
            stat2 = totalDEX;
            multiplier = 1.2;
        }
        else if (job.GetJobType() == JobType.Magician)
        {
            stat1 = totalINT;
            stat2 = totalLUK;
            attack = totalMAD;
            multiplier = 1.0;
        }
        else
        {
            switch (weapon)
            {
                case WeaponType.OneHandedSword:
                case WeaponType.OneHandedAxe:
                case WeaponType.OneHandedMace:
                    stat1 = totalSTR;
                    stat2 = totalDEX;
                    multiplier = 1.20;
                    break;
                case WeaponType.TwoHandedSword:
                case WeaponType.TwoHandedAxe:
                case WeaponType.TwoHandedMace:
                    stat1 = totalSTR;
                    stat2 = totalDEX;
                    multiplier = 1.32;
                    break;
                case WeaponType.Dagger:
                    stat1 = totalLUK;
                    stat2 = totalDEX;
                    stat3 = totalSTR;
                    multiplier = 1.30;
                    break;
                case WeaponType.Barehand:
                    stat1 = totalSTR;
                    stat2 = totalDEX;
                    attack = 1;
                    multiplier = 1.43;
                    break;
                case WeaponType.Polearm:
                case WeaponType.Spear:
                    stat1 = totalSTR;
                    stat2 = totalDEX;
                    multiplier = 1.49;
                    break;
                case WeaponType.Bow:
                    stat1 = totalDEX;
                    stat2 = totalSTR;
                    multiplier = 1.20;
                    break;
                case WeaponType.Crossbow:
                    stat1 = totalDEX;
                    stat2 = totalSTR;
                    multiplier = 1.35;
                    break;
                case WeaponType.ThrowingGlove:
                    stat1 = totalLUK;
                    stat2 = totalDEX;
                    multiplier = 1.75;
                    break;
                case WeaponType.Knuckle:
                    stat1 = totalSTR;
                    stat2 = totalDEX;
                    multiplier = 1.70;
                    break;
                case WeaponType.Gun:
                    stat1 = totalDEX;
                    stat2 = totalSTR;
                    multiplier = 1.50;
                    break;
            }
        }

        var damageMax = (int)((stat3 + stat2 + 4 * stat1) / 100d * attack * multiplier + 0.5);
        var damageMinMultiplier = Math.Min(0.95, weapon.GetMasteryConst() + totalMastery / 100d);
        var damageMin = (int)(damageMinMultiplier * damageMax + 0.5);

        return Tuple.Create(
            damageMin,
            damageMax
        );
    }
}
