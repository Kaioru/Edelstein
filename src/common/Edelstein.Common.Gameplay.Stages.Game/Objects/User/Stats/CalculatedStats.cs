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
        public int ACC { get; private set; }
        public int EVA { get; private set; }

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
            PDD = (int)(INT * 0.4 + 0.5 * LUK + DEX * 0.5 + STR * 1.2);
            MAD = 0;
            MDD = (int)(STR * 0.4 + 0.5 * DEX + LUK * 0.5 + INT * 1.2);
            ACC = 0;
            EVA = 0;

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

            DamageMin = 0;
            DamageMax = 0;
        }

        public async Task Calculate()
        {
            await Reset();
            await CalculateEquipments();
            await CalculatePassiveSkills();
            await CalculateSecondaryStats();
            await CalculateDamage();

            STR += (int)(STR * (STRr / 100d));
            DEX += (int)(DEX * (DEXr / 100d));
            INT += (int)(INT * (INTr / 100d));
            LUK += (int)(LUK * (LUKr / 100d));
            MaxMP += (int)(MaxHP * (MaxHPr / 100d));
            MaxMP += (int)(MaxMP * (MaxMPr / 100d));
            PAD += (int)(PAD * (PADr / 100d));
            PDD += (int)(PDD * (PDDr / 100d));
            MAD += (int)(MAD * (MADr / 100d));
            MDD += (int)(MDD * (MDDr / 100d));
            ACC += (int)(ACC * (ACCr / 100d));
            EVA += (int)(EVA * (EVAr / 100d));

            MaxHP = Math.Min(MaxHP, 99999);
            MaxMP = Math.Min(MaxMP, 99999);

            PAD = Math.Min(PAD, 29999);
            PDD = Math.Min(PDD, 30000);
            MAD = Math.Min(MAD, 29999);
            MDD = Math.Min(MDD, 30000);
            ACC = Math.Min(ACC, 9999);
            EVA = Math.Min(EVA, 9999);
            Speed = Math.Min(Math.Max(Speed, 100), 140);
            Jump = Math.Min(Math.Max(Jump, 100), 123);

            CDMin = Math.Min(CDMin, CDMax);

            DamageMin = Math.Min(DamageMin, DamageMax);
            DamageMin = Math.Min(DamageMin, 999999);
            DamageMax = Math.Min(DamageMax, 999999);
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
            } else
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

            var damageMinMultiplier = weaponType switch
            {
                WeaponType.Wand or
                WeaponType.Staff => 0.25,
                WeaponType.Bow or
                WeaponType.Crossbow or
                WeaponType.ThrowingGlove or
                WeaponType.Gun => 0.15,
                _ => 0.20,
            };
            var mastery = 0; // TODO weapon mastery handling

            DamageMax = (int)((stat3 + stat2 + 4 * stat1) / 100d * attack * multiplier + 0.5);
            DamageMin = (int)(Math.Min(mastery / 100d + damageMinMultiplier, 0.95) * DamageMax + 0.5);
        }
    }
}
