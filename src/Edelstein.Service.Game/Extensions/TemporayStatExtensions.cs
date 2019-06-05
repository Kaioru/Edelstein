using System.Collections.Generic;
using Edelstein.Core.Types;
using Edelstein.Provider.Templates.Item.Consume;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Service.Game.Fields.Objects.User.Stats;

namespace Edelstein.Service.Game.Extensions
{
    public static class TemporayStatExtensions
    {
        public static IDictionary<TemporaryStatType, short> GetTemporaryStats(this SkillLevelTemplate template)
        {
            var stats = new Dictionary<TemporaryStatType, short>();

            if (template.PAD != 0) stats.Add(TemporaryStatType.PAD, template.PAD);
            if (template.PDD != 0) stats.Add(TemporaryStatType.PDD, template.PDD);
            if (template.MAD != 0) stats.Add(TemporaryStatType.MAD, template.MAD);
            if (template.MDD != 0) stats.Add(TemporaryStatType.MDD, template.MDD);
            if (template.ACC != 0) stats.Add(TemporaryStatType.ACC, template.ACC);
            if (template.EVA != 0) stats.Add(TemporaryStatType.EVA, template.EVA);
            if (template.Craft != 0) stats.Add(TemporaryStatType.Craft, template.Craft);
            if (template.Speed != 0) stats.Add(TemporaryStatType.Speed, template.Speed);
            if (template.Jump != 0) stats.Add(TemporaryStatType.Jump, template.Jump);

            if (template.Morph > 0) stats.Add(TemporaryStatType.Morph, template.Morph);

            if (template.EMHP != 0) stats.Add(TemporaryStatType.EMHP, template.EMHP);
            if (template.EMMP != 0) stats.Add(TemporaryStatType.EMMP, template.EMMP);
            if (template.EPAD != 0) stats.Add(TemporaryStatType.EPAD, template.EPAD);
            if (template.EPDD != 0) stats.Add(TemporaryStatType.EPDD, template.EPDD);
            // if (template.EMAD != 0) temporaryStats.Add(TemporaryStatType.EMAD, template.EMAD);
            if (template.EMDD != 0) stats.Add(TemporaryStatType.EMDD, template.EMDD);

            switch ((Skill) template.SkillID)
            {
                case Skill.MagicianMagicGuard:
                case Skill.FlamewizardMagicGuard:
                case Skill.EvanMagicGuard:
                    stats.Add(TemporaryStatType.MagicGuard, template.X);
                    break;
                case Skill.RogueDarkSight:
                case Skill.NightwalkerDarkSight:
                    stats.Add(TemporaryStatType.DarkSight, template.X);
                    break;
                case Skill.FighterWeaponBooster:
                case Skill.PageWeaponBooster:
                case Skill.SpearmanWeaponBooster:
                case Skill.Mage1MagicBooster:
                case Skill.Mage2MagicBooster:
                case Skill.HunterBowBooster:
                case Skill.CrossbowmanCrossbowBooster:
                case Skill.AssassinJavelinBooster:
                case Skill.ThiefDaggerBooster:
                case Skill.Dual1DualBooster:
                case Skill.InfighterKnuckleBooster:
                case Skill.GunslingerGunBooster:
                case Skill.StrikerKnuckleBooster:
                case Skill.SoulmasterSwordBooster:
                case Skill.FlamewizardMagicBooster:
                case Skill.WindbreakerBowBooster:
                case Skill.NightwalkerJavelinBooster:
                case Skill.AranPolearmBooster:
                case Skill.EvanMagicBooster:
                case Skill.BmageStaffBooster:
                case Skill.WildhunterCrossbowBooster:
                case Skill.MechanicBooster:
                    stats.Add(TemporaryStatType.Booster, template.X);
                    break;
                case Skill.FighterPowerGuard:
                case Skill.PagePowerGuard:
                    stats.Add(TemporaryStatType.PowerGuard, template.X);
                    break;
                case Skill.NoviceHyperBody:
                case Skill.SpearmanHyperBody:
                case Skill.AdminHyperBody:
                case Skill.NoblesseHyperBody:
                case Skill.LegendHyperBody:
                case Skill.EvanjrHyperBody:
                case Skill.CitizenHyperBody:
                    stats.Add(TemporaryStatType.MaxHP, template.X);
                    stats.Add(TemporaryStatType.MaxMP, template.Y);
                    break;
                case Skill.ClericInvincible:
                    stats.Add(TemporaryStatType.Invincible, template.X);
                    break;
                case Skill.HunterSoulArrowBow:
                case Skill.CrossbowmanSoulArrowCrossbow:
                case Skill.WindbreakerSoulArrowBow:
                case Skill.WildhunterSoulArrowCrossbow:
                    stats.Add(TemporaryStatType.SoulArrow, template.X);
                    break;
                // TODO: Combo attack
                // TODO: Weapon charge
                case Skill.DragonknightDragonBlood:
                    stats.Add(TemporaryStatType.DragonBlood, template.X);
                    break;
                case Skill.PriestHolySymbol:
                case Skill.AdminHolySymbol:
                    stats.Add(TemporaryStatType.HolySymbol, template.X);
                    break;
                case Skill.HermitMesoUp:
                    stats.Add(TemporaryStatType.MesoUp, template.X);
                    break;
                case Skill.HermitShadowPartner:
                case Skill.ThiefmasterShadowPartner:
                case Skill.Dual4MirrorImaging:
                case Skill.NightwalkerShadowPartner:
                    stats.Add(TemporaryStatType.ShadowPartner, template.X);
                    break;
                case Skill.ThiefmasterPickpocket:
                    stats.Add(TemporaryStatType.PickPocket, template.X);
                    break;
                case Skill.ThiefmasterMesoGuard:
                    stats.Add(TemporaryStatType.MesoGuard, template.X);
                    break;
                case Skill.HeroMapleHero:
                case Skill.PaladinMapleHero:
                case Skill.DarkknightMapleHero:
                case Skill.Archmage1MapleHero:
                case Skill.Archmage2MapleHero:
                case Skill.BishopMapleHero:
                case Skill.BowmasterMapleHero:
                case Skill.CrossbowmasterMapleHero:
                case Skill.NightlordMapleHero:
                case Skill.ShadowerMapleHero:
                case Skill.Dual5MapleHero:
                case Skill.ViperMapleHero:
                case Skill.CaptainMapleHero:
                case Skill.AranMapleHero:
                case Skill.EvanMapleHero:
                case Skill.BmageMapleHero:
                case Skill.WildhunterMapleHero:
                case Skill.MechanicMapleHero:
                    stats.Add(TemporaryStatType.BasicStatUp, template.X);
                    break;
                case Skill.HeroStance:
                case Skill.PaladinStance:
                case Skill.DarkknightStance:
                case Skill.AranFreezeStanding:
                case Skill.BmageStance:
                    stats.Add(TemporaryStatType.Stance, template.X);
                    break;
                // TODO: sharp eyes
                case Skill.Archmage1ManaReflection:
                case Skill.Archmage2ManaReflection:
                case Skill.BishopManaReflection:
                    stats.Add(TemporaryStatType.ManaReflection, template.X);
                    break;
                case Skill.NightlordSpiritJavelin:
                    stats.Add(TemporaryStatType.SpiritJavelin, template.X);
                    break;
                case Skill.Archmage1Infinity:
                case Skill.Archmage2Infinity:
                case Skill.BishopInfinity:
                    stats.Add(TemporaryStatType.Infinity, template.X);
                    break;
                case Skill.BishopHolyShield:
                    stats.Add(TemporaryStatType.Holyshield, template.X);
                    break;
                case Skill.BowmasterHamstring:
                    stats.Add(TemporaryStatType.HamString, template.X);
                    break;
                case Skill.CrossbowmasterBlind:
                case Skill.WildhunterBlind:
                    stats.Add(TemporaryStatType.Blind, template.X);
                    break;
                case Skill.BowmasterConcentration:
                    stats.Add(TemporaryStatType.Concentration, template.X);
                    break;
                case Skill.NoviceMaxlevelEchobuff:
                case Skill.NoblesseMaxlevelEchobuff:
                case Skill.LegendMaxlevelEchobuff:
                case Skill.EvanjrMaxlevelEchobuff:
                case Skill.CitizenMaxlevelEchobuff:
                    stats.Add(TemporaryStatType.MaxLevelBuff, template.X);
                    break;
            }

            return stats;
        }

        public static IDictionary<TemporaryStatType, short> GetTemporaryStats(this StatChangeItemTemplate template)
        {
            var stats = new Dictionary<TemporaryStatType, short>();

            if (template.PAD != 0) stats.Add(TemporaryStatType.PAD, template.PAD);
            if (template.PDD != 0) stats.Add(TemporaryStatType.PDD, template.PDD);
            if (template.MAD != 0) stats.Add(TemporaryStatType.MAD, template.MAD);
            if (template.MDD != 0) stats.Add(TemporaryStatType.MDD, template.MDD);
            if (template.ACC != 0) stats.Add(TemporaryStatType.ACC, template.ACC);
            if (template.EVA != 0) stats.Add(TemporaryStatType.EVA, template.EVA);
            if (template.Craft != 0) stats.Add(TemporaryStatType.Craft, template.Craft);
            if (template.Speed != 0) stats.Add(TemporaryStatType.Speed, template.Speed);
            if (template.Jump != 0) stats.Add(TemporaryStatType.Jump, template.Jump);

            if (template.Morph > 0) stats.Add(TemporaryStatType.Morph, template.Morph);

            return stats;
        }
    }
}