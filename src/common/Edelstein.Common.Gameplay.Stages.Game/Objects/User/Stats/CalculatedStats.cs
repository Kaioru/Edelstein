using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Common.Gameplay.Users.Skills.Templates;

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
            DEXr += (int)(DEX * (DEXr / 100d));
            INTr += (int)(INT * (INTr / 100d));
            LUKr += (int)(LUK * (LUKr / 100d));
            MaxMPr += (int)(MaxMP * (MaxMPr / 100d));
            MaxMPr += (int)(MaxMP * (MaxMPr / 100d));
            PADr += (int)(PAD * (PADr / 100d));
            PDDr += (int)(PDD * (PDDr / 100d));
            MADr += (int)(MAD * (MADr / 100d));
            MDDr += (int)(MDD * (MDDr / 100d));
            ACCr += (int)(ACC * (ACCr / 100d));
            EVAr += (int)(EVA * (EVAr / 100d));

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

        public async Task CalculateEquipments() { }
        public async Task CalculatePassiveSkills() { }
        public async Task CalculateSecondaryStats() { }
        public async Task CalculateDamage() { }
    }
}
