using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Common.Gameplay.Users.Skills.Templates;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using System.Linq;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Constants.Types;
using Edelstein.Common.Gameplay.Users;
using System.Collections.Generic;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class CalculatedStats : ICalculatedStats
    {
        public int STR { get; private set; }
        public int DEX { get; private set; }
        public int INT { get; private set; }
        public int LUK { get; private set; }
        public int MaxHP { get; private set; }
        public int MaxMP { get; private set; }
        public int PAD { get; private set; }
        public int PDD { get; private set; }
        public int MAD { get; private set; }
        public int MDD { get; private set; }
        public int PACC { get; private set; }
        public int MACC { get; private set; }
        public int PEVA { get; private set; }
        public int MEVA { get; private set; }

        private int ACC { get; set; }
        private int EVA { get; set; }

        public int Ar { get; private set; }
        public int Er { get; private set; }

        private int STRr { get; set; }
        private int DEXr { get; set; }
        private int INTr { get; set; }
        private int LUKr { get; set; }
        private int MaxHPr { get; set; }
        private int MaxMPr { get; set; }
        private int PADr { get; set; }
        private int PDDr { get; set; }
        private int MADr { get; set; }
        private int MDDr { get; set; }
        private int ACCr { get; set; }
        private int EVAr { get; set; }

        public int Craft { get; private set; }
        public int Speed { get; private set; }
        public int Jump { get; private set; }

        public int Cr { get; private set; }
        public int CDMin { get; private set; }
        public int CDMax { get; private set; }

        public int IMDr { get; private set; }

        public int PDamR { get; private set; }
        public int MDamR { get; private set; }
        public int BossDamR { get; private set; }

        private int Mastery { get; set; }

        public int DamageMin { get; private set; }
        public int DamageMax { get; private set; }

        private readonly IFieldObjUser _user;

        private readonly ITemplateRepository<ItemTemplate> _itemTemplates;
        private readonly ITemplateRepository<ItemOptionTemplate> _optionTemplates;
        private readonly ITemplateRepository<ItemSetTemplate> _setTemplates;
        private readonly ITemplateRepository<SkillTemplate> _skillTemplates;

        public CalculatedStats(
            IFieldObjUser user,
            ITemplateRepository<ItemTemplate> itemTemplates,
            ITemplateRepository<ItemOptionTemplate> optionTemplates,
            ITemplateRepository<ItemSetTemplate> setTemplates,
            ITemplateRepository<SkillTemplate> skillTemplates
        )
        {
            _user = user;
            _itemTemplates = itemTemplates;
            _optionTemplates = optionTemplates;
            _setTemplates = setTemplates;
            _skillTemplates = skillTemplates;
        }

        public async Task Reset()
        {
            var character = _user.Character;

            STR = character.STR;
            DEX = character.DEX;
            INT = character.INT;
            LUK = character.LUK;
            MaxHP = character.MaxHP;
            MaxMP = character.MaxMP;
            PAD = 0;
            PDD = 0;
            MAD = 0;
            MDD = 0;
            PACC = 0;
            MACC = 0;
            PEVA = 0;
            MEVA = 0;

            ACC = 0;
            EVA = 0;

            Ar = 0;
            Er = 0;

            STRr = 0;
            DEXr = 0;
            INTr = 0;
            LUKr = 0;
            MaxHPr = 0;
            MaxMPr = 0;
            PADr = 0;
            PDDr = 0;
            MADr = 0;
            MDDr = 0;
            ACCr = 0;
            EVAr = 0;

            Craft = INT + DEX + LUK;
            Speed = 100;
            Jump = 100;

            Cr = 5;
            CDMin = 20;
            CDMax = 50;

            IMDr = 0;

            PDamR = 0;
            MDamR = 0;
            BossDamR = 0;

            Mastery = 0;

            DamageMin = 1;
            DamageMax = 1;
        }

        public async Task Calculate()
        {
            await Reset();
            await CalculateEquipments();
            await CalculatePassiveSkills();
            await CalculateSecondaryStats();
            await CalculateMastery();

            STR += (int)(STR * (STRr / 100d));
            DEX += (int)(DEX * (DEXr / 100d));
            INT += (int)(INT * (INTr / 100d));
            LUK += (int)(LUK * (LUKr / 100d));
            MaxHP += (int)(MaxHP * (MaxHPr / 100d));
            MaxMP += (int)(MaxMP * (MaxMPr / 100d));

            PAD += (int)(PAD * (PADr / 100d));
            PDD += (int)(STR * 1.2 + LUK * 0.5 + DEX * 0.5 + INT * 0.4);
            PDD += (int)(PDD * (PDDr / 100d));
            MAD += (int)(MAD * (MADr / 100d));
            MDD += (int)(INT * 1.2 + DEX * 0.5 + LUK * 0.5 + STR * 0.4);
            MDD += (int)(MDD * (MDDr / 100d));
            PACC = (int)(DEX * 1.2 + LUK) + ACC;
            PACC += (int)(PACC * (ACCr / 100d));
            MACC = (int)(LUK * 1.2 + INT) + ACC;
            MACC += (int)(MACC * (ACCr / 100d));
            PEVA = LUK * 2 + DEX + EVA;
            PEVA += (int)(PEVA * (EVAr / 100d));
            MEVA = LUK * 2 + INT + EVA;
            MEVA += (int)(MEVA * (EVAr / 100d));

            MaxHP = Math.Min(MaxHP, 99999);
            MaxMP = Math.Min(MaxMP, 99999);

            PAD = Math.Min(PAD, 29999);
            PDD = Math.Min(PDD, 30000);
            MAD = Math.Min(MAD, 29999);
            MDD = Math.Min(MDD, 30000);
            PACC = Math.Min(PACC, 9999);
            MACC = Math.Min(MACC, 9999);
            PEVA = Math.Min(PEVA, 9999);
            MEVA = Math.Min(MEVA, 9999);
            Speed = Math.Min(Math.Max(Speed, 100), 140);
            Jump = Math.Min(Math.Max(Jump, 100), 123);

            await CalculateDamage();

            CDMin = Math.Min(CDMin, CDMax);

            DamageMin = Math.Min(DamageMin, DamageMax);
            DamageMin = Math.Min(Math.Max(DamageMin, 1), 999999);
            DamageMax = Math.Min(Math.Max(DamageMax, 1), 999999);
        }

        public async Task CalculateEquipments()
        {
            var equippedItems = _user.Character.Inventories[ItemInventoryType.Equip].Items
                .Where(kv => kv.Key < 0)
                .Where(kv => kv.Value is ItemSlotEquip)
                .Select(kv => (kv.Key, (ItemSlotEquip)kv.Value))
                .ToList();

            await Task.WhenAll(equippedItems.Select(async kv =>
            {
                var (pos, item) = kv;
                var template = await _itemTemplates.Retrieve(item.TemplateID);

                if (template is not ItemEquipTemplate equipTemplate) return;

                STR += item.STR;
                DEX += item.DEX;
                INT += item.INT;
                LUK += item.LUK;
                MaxHP += item.MaxHP;
                MaxMP += item.MaxMP;

                if ( // TODO use BodyPart
                    pos != -30 &&
                    pos != -38 &&
                    pos != -31 &&
                    pos != -39 &&
                    pos != -32 &&
                    pos != -40 &&
                    (item.TemplateID / 10000 == 190 || pos != -18 && pos != -19 && pos != -20))
                {
                    PAD += item.PAD;
                    PDD += item.PDD;
                    MAD += item.MAD;
                    MDD += item.MDD;
                    ACC += item.ACC;
                    EVA += item.EVA;
                    Craft += item.Craft;
                    Speed += item.Speed;
                    Jump += item.Jump;
                }

                MaxHPr += equipTemplate.IncMaxHPr;
                MaxMPr += equipTemplate.IncMaxMPr;

                // TODO: and not Dragon or Mechanic
                // TODO: item options
            }));

            // TODO: item sets
        }

        public async Task CalculatePassiveSkills()
        {
            var skills = _user.Character.SkillRecord;

            await Task.WhenAll(skills.Select(async kv =>
            {
                var (id, record) = kv;
                var skillTemplate = await _skillTemplates.Retrieve(id);

                if (skillTemplate == null) return;
                if (!skillTemplate.IsPSD) return;
                if (!skillTemplate.LevelData.ContainsKey(kv.Value.Level)) return;

                var skillLevelTemplate = skillTemplate.LevelData[kv.Value.Level];

                // TODO: more psd handling

                MaxHPr += skillLevelTemplate.MHPr;
                MaxMPr += skillLevelTemplate.MMPr;

                Cr += skillLevelTemplate.Cr;
                CDMin += skillLevelTemplate.CDMin;
                CDMax += skillLevelTemplate.CDMax;

                ACCr += skillLevelTemplate.ACCr;
                EVAr += skillLevelTemplate.EVAr;

                Ar += skillLevelTemplate.Ar;
                Er += skillLevelTemplate.Er;

                PADr += skillLevelTemplate.PADr;
                MADr += skillLevelTemplate.MADr;

                PAD += skillLevelTemplate.PADx;
                MAD += skillLevelTemplate.MADx;

                IMDr += skillLevelTemplate.IMDr;

                Jump += skillLevelTemplate.PsdJump;
                Speed += skillLevelTemplate.PsdSpeed;
            }));
        }

        public async Task CalculateSecondaryStats()
        {
            PAD += _user.SecondaryStats.GetValue(SecondaryStatType.PAD);
            PDD += _user.SecondaryStats.GetValue(SecondaryStatType.PDD);
            MAD += _user.SecondaryStats.GetValue(SecondaryStatType.MAD);
            MDD += _user.SecondaryStats.GetValue(SecondaryStatType.MDD);
            ACC += _user.SecondaryStats.GetValue(SecondaryStatType.ACC);
            EVA += _user.SecondaryStats.GetValue(SecondaryStatType.EVA);
            Craft += _user.SecondaryStats.GetValue(SecondaryStatType.Craft);
            Speed += _user.SecondaryStats.GetValue(SecondaryStatType.Speed);
            Jump += _user.SecondaryStats.GetValue(SecondaryStatType.Jump);
            MaxHP += _user.SecondaryStats.GetValue(SecondaryStatType.MaxHP);
            MaxMP += _user.SecondaryStats.GetValue(SecondaryStatType.MaxMP);

            MaxHP += _user.SecondaryStats.GetValue(SecondaryStatType.EMHP);
            MaxMP += _user.SecondaryStats.GetValue(SecondaryStatType.EMMP);
            PAD += _user.SecondaryStats.GetValue(SecondaryStatType.EPAD);
            PDD += _user.SecondaryStats.GetValue(SecondaryStatType.EPDD);
            // MAD += _user.SecondaryStats.GetValue(SecondaryStatType.EMAD); // Does not exist
            MDD += _user.SecondaryStats.GetValue(SecondaryStatType.EMDD);
        }

        public async Task CalculateMastery()
        {
            var equipInventory = _user.Character.Inventories[ItemInventoryType.Equip];
            var weaponID = equipInventory.Items.ContainsKey(-(short)BodyPart.Weapon)
                ? equipInventory.Items[-(short)BodyPart.Weapon].TemplateID
                : 0;
            var subWeaponID = equipInventory.Items.ContainsKey(-(short)BodyPart.Shield)
                ? equipInventory.Items[-(short)BodyPart.Weapon].TemplateID
                : 0;
            var jobType = GameConstants.GetJobType(_user.Character.Job);
            var jobRace = GameConstants.GetJobRace(_user.Character.Job);
            var jobBranch = GameConstants.GetJobBranch(_user.Character.Job);
            var weaponType = GameConstants.GetWeaponType(weaponID);
            var subWeaponType = GameConstants.GetWeaponType(subWeaponID);

            if (jobType == 2)
            {
                var skills = new List<int>();

                switch (jobRace)
                {
                    case 0:
                        skills.Add(jobBranch switch
                        {
                            1 => (int)Skill.Wizard1SpellMastery,
                            2 => (int)Skill.Wizard2SpellMastery,
                            3 => (int)Skill.ClericSpellMastery,
                            _ => 0
                        });
                        break;
                    case 1:
                        skills.Add((int)Skill.FlamewizardSpellMastery);
                        break;
                    case 2:
                        skills.Add((int)Skill.EvanSpellMastery);
                        skills.Add((int)Skill.EvanMagicMastery);
                        break;
                    case 3:
                        skills.Add((int)Skill.BmageSpellMastery);
                        break;
                }

                foreach (var skill in skills)
                {
                    var skillTemplate = await _skillTemplates.Retrieve(skill);
                    var skillLevelTemplate = skillTemplate.LevelData[_user.Character.GetSkillLevel(skill)];

                    Mastery += skillLevelTemplate.Mastery;
                    MAD += skillLevelTemplate.X;
                }
                return;
            }

            void GetMastery(int skillID, ref int mastery, ref int stat)
            {
                var skillLevel = _user.Character.GetSkillLevel(skillID);
                var skillTemplate = _skillTemplates.Retrieve(skillID).Result;
                var skillLevelTemplate = skillTemplate.LevelData[skillLevel];

                mastery += skillLevelTemplate.Mastery;
                stat += skillLevelTemplate.X;
            }

            var incMastery = 0;
            var incPAD = 0;
            var incACC = 0;
            var incIgnore = 0;

            switch (weaponType)
            {
                case WeaponType.OneHandedSword:
                case WeaponType.TwoHandedSword:
                    {
                        var skills = new List<int>{
                            (int)Skill.FighterWeaponMastery,
                            (int)Skill.PageWeaponMastery,
                            (int)Skill.SoulmasterSwordMastery
                        };

                        foreach (var skill in skills)
                        {
                            if (incMastery != 0) continue;

                            GetMastery(skill, ref incMastery, ref incACC);

                            if (skill == (int)Skill.PageWeaponMastery && _user.SecondaryStats.HasStat(SecondaryStatType.WeaponCharge))
                                GetMastery((int)Skill.PaladinAdvancedCharge, ref incMastery, ref incIgnore);
                            break;
                        }
                        break;
                    }
                case WeaponType.OneHandedAxe:
                case WeaponType.TwoHandedAxe:
                    GetMastery((int)Skill.FighterWeaponMastery, ref incMastery, ref incACC);
                    break;
                case WeaponType.OneHandedMace:
                case WeaponType.TwoHandedMace:
                    {
                        GetMastery((int)Skill.PageWeaponMastery, ref incMastery, ref incACC);

                        if (_user.SecondaryStats.HasStat(SecondaryStatType.WeaponCharge))
                            GetMastery((int)Skill.PaladinAdvancedCharge, ref incMastery, ref incIgnore);
                        break;
                    }
                case WeaponType.Spear:
                case WeaponType.Polearm:
                    {
                        if (weaponType == WeaponType.Polearm && jobRace == 2 && jobType == 1)
                        {
                            GetMastery((int)Skill.AranPolearmMastery, ref incMastery, ref incACC);
                            GetMastery((int)Skill.AranHighMastery, ref incMastery, ref incPAD);
                            break;
                        }

                        GetMastery((int)Skill.SpearmanWeaponMastery, ref incMastery, ref incACC);

                        if (_user.SecondaryStats.HasStat(SecondaryStatType.Beholder))
                            GetMastery((int)Skill.DarkknightBeholder, ref incMastery, ref incIgnore);
                        break;
                    }
                case WeaponType.Bow:
                    {
                        if (jobRace == 1)
                        {
                            GetMastery((int)Skill.WindbreakerBowMastery, ref incMastery, ref incACC);
                            GetMastery((int)Skill.WindbreakerBowExpert, ref incMastery, ref incPAD);
                            break;
                        }

                        GetMastery((int)Skill.HunterBowMastery, ref incMastery, ref incACC);
                        GetMastery((int)Skill.BowmasterBowExpert, ref incMastery, ref incPAD);
                        break;
                    }
                case WeaponType.Crossbow:
                    {
                        if (jobRace == 2)
                        {
                            GetMastery((int)Skill.WildhunterCrossbowMastery, ref incMastery, ref incACC);
                            GetMastery((int)Skill.WildhunterCrossbowExpert, ref incMastery, ref incPAD);
                            break;
                        }

                        GetMastery((int)Skill.CrossbowmanCrossbowMastery, ref incMastery, ref incACC);
                        GetMastery((int)Skill.CrossbowmasterCrossbowExpert, ref incMastery, ref incPAD);
                        break;
                    }
                case WeaponType.ThrowingGlove:
                    {
                        if (jobRace == 1)
                        {
                            GetMastery((int)Skill.NightwalkerJavelinMastery, ref incMastery, ref incACC);
                            break;
                        }

                        GetMastery((int)Skill.AssassinJavelinMastery, ref incMastery, ref incACC);
                        break;
                    }
                case WeaponType.Dagger:
                    {
                        if (subWeaponType == WeaponType.SubDagger)
                        {
                            GetMastery((int)Skill.Dual1DualMastery, ref incMastery, ref incACC);
                            break;
                        }

                        GetMastery((int)Skill.ThiefDaggerMastery, ref incMastery, ref incACC);
                        break;
                    }
                case WeaponType.Knuckle:
                    {
                        if (jobRace == 1)
                        {
                            GetMastery((int)Skill.StrikerKnuckleMastery, ref incMastery, ref incACC);
                            break;
                        }

                        GetMastery((int)Skill.InfighterKnuckleMastery, ref incMastery, ref incACC);
                        break;
                    }
                case WeaponType.Gun:
                    {
                        if (jobRace == 2)
                        {
                            GetMastery((int)Skill.MechanicGunMastery, ref incMastery, ref incACC);
                            GetMastery((int)Skill.MechanicHn07Upgrade, ref incMastery, ref incIgnore);
                            break;
                        }

                        GetMastery((int)Skill.GunslingerGunMastery, ref incMastery, ref incACC);
                        break;
                    }
            }

            Mastery += incMastery;
            PAD += incPAD;
            MAD += incACC;
        }

        public async Task CalculateDamage()
        {
            var equipInventory = _user.Character.Inventories[ItemInventoryType.Equip];
            var weaponID = equipInventory.Items.ContainsKey(-(short)BodyPart.Weapon)
                ? equipInventory.Items[-(short)BodyPart.Weapon].TemplateID
                : 0;
            var weaponType = GameConstants.GetWeaponType(weaponID);
            var stat1 = 0;
            var stat2 = 0;
            var stat3 = 0;
            var attack = PAD;
            var multiplier = 0.0;

            if (GameConstants.GetJobLevel(_user.Character.Job) == 0)
            {
                stat1 = STR;
                stat2 = DEX;
                multiplier = 1.2;
            }
            else if (GameConstants.GetJobType(_user.Character.Job) == 2)
            {
                stat1 = INT;
                stat2 = LUK;
                attack = MAD;
                multiplier = 1.0;
            }
            else
            {
                switch (weaponType)
                {
                    case WeaponType.OneHandedSword:
                    case WeaponType.OneHandedAxe:
                    case WeaponType.OneHandedMace:
                        stat1 = STR;
                        stat2 = DEX;
                        multiplier = 1.20;
                        break;
                    case WeaponType.TwoHandedSword:
                    case WeaponType.TwoHandedAxe:
                    case WeaponType.TwoHandedMace:
                        stat1 = STR;
                        stat2 = DEX;
                        multiplier = 1.32;
                        break;
                    case WeaponType.Dagger:
                        stat1 = LUK;
                        stat2 = DEX;
                        stat3 = STR;
                        multiplier = 1.30;
                        break;
                    case WeaponType.Barehand:
                        stat1 = STR;
                        stat2 = DEX;
                        attack = 1;
                        multiplier = 1.43;
                        break;
                    case WeaponType.Polearm:
                    case WeaponType.Spear:
                        stat1 = STR;
                        stat2 = DEX;
                        multiplier = 1.49;
                        break;
                    case WeaponType.Bow:
                        stat1 = DEX;
                        stat2 = STR;
                        multiplier = 1.20;
                        break;
                    case WeaponType.Crossbow:
                        stat1 = DEX;
                        stat2 = STR;
                        multiplier = 1.35;
                        break;
                    case WeaponType.ThrowingGlove:
                        stat1 = LUK;
                        stat2 = DEX;
                        multiplier = 1.75;
                        break;
                    case WeaponType.Knuckle:
                        stat1 = STR;
                        stat2 = DEX;
                        multiplier = 1.70;
                        break;
                    case WeaponType.Gun:
                        stat1 = DEX;
                        stat2 = STR;
                        multiplier = 1.50;
                        break;
                }
            }

            var masteryMultiplier = weaponType switch
            {
                WeaponType.Wand or
                WeaponType.Staff => 0.25,
                WeaponType.Bow or
                WeaponType.Crossbow or
                WeaponType.ThrowingGlove or
                WeaponType.Gun => 0.15,
                _ => 0.20,
            };

            masteryMultiplier += Mastery / 100d;
            masteryMultiplier = Math.Min(masteryMultiplier, 0.95);

            DamageMax = (int)((stat3 + stat2 + 4 * stat1) / 100d * attack * multiplier + 0.5);
            DamageMin = (int)(DamageMax * masteryMultiplier + 0.5);
        }
    }
}
