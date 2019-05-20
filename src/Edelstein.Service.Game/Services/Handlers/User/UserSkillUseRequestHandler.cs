using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Types;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Effects.Types;
using Edelstein.Service.Game.Fields.Objects.User.Stats;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserSkillUseRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var templateID = packet.Decode<int>();
            var template = user.Service.TemplateManager.Get<SkillTemplate>(templateID);
            var skillLevel = user.Character.GetSkillLevel(templateID);

            if (template == null) return;
            if (skillLevel <= 0) return;

            var stats = new Dictionary<TemporaryStatType, short>();
            var level = template.LevelData[skillLevel];

            if (level.PAD != 0) stats.Add(TemporaryStatType.PAD, level.PAD);
            if (level.PDD != 0) stats.Add(TemporaryStatType.PDD, level.PDD);
            if (level.MAD != 0) stats.Add(TemporaryStatType.MAD, level.MAD);
            if (level.MDD != 0) stats.Add(TemporaryStatType.MDD, level.MDD);
            if (level.ACC != 0) stats.Add(TemporaryStatType.ACC, level.ACC);
            if (level.EVA != 0) stats.Add(TemporaryStatType.EVA, level.EVA);
            if (level.Craft != 0) stats.Add(TemporaryStatType.Craft, level.Craft);
            if (level.Speed != 0) stats.Add(TemporaryStatType.Speed, level.Speed);
            if (level.Jump != 0) stats.Add(TemporaryStatType.Jump, level.Jump);

            if (level.Morph > 0) stats.Add(TemporaryStatType.Morph, level.Morph);

            if (level.EMHP != 0) stats.Add(TemporaryStatType.EMHP, level.EMHP);
            if (level.EMMP != 0) stats.Add(TemporaryStatType.EMMP, level.EMMP);
            if (level.EPAD != 0) stats.Add(TemporaryStatType.EPAD, level.EPAD);
            if (level.EPDD != 0) stats.Add(TemporaryStatType.EPDD, level.EPDD);
            // if (level.EMAD != 0) temporaryStats.Add(TemporaryStatType.EMAD, level.EMAD);
            if (level.EMDD != 0) stats.Add(TemporaryStatType.EMDD, level.EMDD);

            switch ((Skill) templateID)
            {
                case Skill.MagicianMagicGuard:
                case Skill.FlamewizardMagicGuard:
                case Skill.EvanMagicGuard:
                    stats.Add(TemporaryStatType.MagicGuard, level.X);
                    break;
                case Skill.RogueDarkSight:
                case Skill.NightwalkerDarkSight:
                    stats.Add(TemporaryStatType.DarkSight, level.X);
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
                    stats.Add(TemporaryStatType.Booster, level.X);
                    break;
                case Skill.FighterPowerGuard:
                case Skill.PagePowerGuard:
                    stats.Add(TemporaryStatType.PowerGuard, level.X);
                    break;
                case Skill.NoviceHyperBody:
                case Skill.SpearmanHyperBody:
                case Skill.AdminHyperBody:
                case Skill.NoblesseHyperBody:
                case Skill.LegendHyperBody:
                case Skill.EvanjrHyperBody:
                case Skill.CitizenHyperBody:
                    stats.Add(TemporaryStatType.MaxHP, level.X);
                    stats.Add(TemporaryStatType.MaxMP, level.Y);
                    break;
                case Skill.ClericInvincible:
                    stats.Add(TemporaryStatType.Invincible, level.X);
                    break;
                case Skill.HunterSoulArrowBow:
                case Skill.CrossbowmanSoulArrowCrossbow:
                case Skill.WindbreakerSoulArrowBow:
                case Skill.WildhunterSoulArrowCrossbow:
                    stats.Add(TemporaryStatType.SoulArrow, level.X);
                    break;
                // TODO: Combo attack
                // TODO: Weapon charge
                case Skill.DragonknightDragonBlood:
                    stats.Add(TemporaryStatType.DragonBlood, level.X);
                    break;
                case Skill.PriestHolySymbol:
                case Skill.AdminHolySymbol:
                    stats.Add(TemporaryStatType.HolySymbol, level.X);
                    break;
                case Skill.HermitMesoUp:
                    stats.Add(TemporaryStatType.MesoUp, level.X);
                    break;
                case Skill.HermitShadowPartner:
                case Skill.ThiefmasterShadowPartner:
                case Skill.Dual4MirrorImaging:
                case Skill.NightwalkerShadowPartner:
                    stats.Add(TemporaryStatType.ShadowPartner, level.X);
                    break;
                case Skill.ThiefmasterPickpocket:
                    stats.Add(TemporaryStatType.PickPocket, level.X);
                    break;
                case Skill.ThiefmasterMesoGuard:
                    stats.Add(TemporaryStatType.MesoGuard, level.X);
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
                    stats.Add(TemporaryStatType.BasicStatUp, level.X);
                    break;
                case Skill.HeroStance:
                case Skill.PaladinStance:
                case Skill.DarkknightStance:
                case Skill.AranFreezeStanding:
                case Skill.BmageStance:
                    stats.Add(TemporaryStatType.Stance, level.X);
                    break;
                // TODO: sharp eyes
                case Skill.Archmage1ManaReflection:
                case Skill.Archmage2ManaReflection:
                case Skill.BishopManaReflection:
                    stats.Add(TemporaryStatType.ManaReflection, level.X);
                    break;
                case Skill.NightlordSpiritJavelin:
                    stats.Add(TemporaryStatType.SpiritJavelin, level.X);
                    break;
                case Skill.Archmage1Infinity:
                case Skill.Archmage2Infinity:
                case Skill.BishopInfinity:
                    stats.Add(TemporaryStatType.Infinity, level.X);
                    break;
                case Skill.BishopHolyShield:
                    stats.Add(TemporaryStatType.Holyshield, level.X);
                    break;
                case Skill.BowmasterHamstring:
                    stats.Add(TemporaryStatType.HamString, level.X);
                    break;
                case Skill.CrossbowmasterBlind:
                case Skill.WildhunterBlind:
                    stats.Add(TemporaryStatType.Blind, level.X);
                    break;
                case Skill.BowmasterConcentration:
                    stats.Add(TemporaryStatType.Concentration, level.X);
                    break;
                case Skill.NoviceMaxlevelEchobuff:
                case Skill.NoblesseMaxlevelEchobuff:
                case Skill.LegendMaxlevelEchobuff:
                case Skill.EvanjrMaxlevelEchobuff:
                case Skill.CitizenMaxlevelEchobuff:
                    stats.Add(TemporaryStatType.MaxLevelBuff, level.X);
                    break;
            }

            if (stats.Count > 0)
            {
                await user.ModifyTemporaryStats(ts =>
                {
                    if (level.Time > 0)
                    {
                        var expire = DateTime.Now.AddSeconds(level.Time);
                        stats.ForEach(t => ts.Set(t.Key, templateID, t.Value, expire));
                    }
                    else stats.ForEach(t => ts.Set(t.Key, templateID, t.Value));
                });
            }
            // TODO: party/map buffs

            await user.Effect(new SkillUseEffect(templateID, (byte) skillLevel), local: false, remote: true);
            await user.ModifyStats(exclRequest: true);
        }
    }
}