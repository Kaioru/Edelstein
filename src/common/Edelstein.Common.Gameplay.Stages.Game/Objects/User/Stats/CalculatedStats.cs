using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
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
        public int Craft { get; private set; }
        public int Speed { get; private set; }
        public int Jump { get; private set; }

        public int STRr { get; private set; }
        public int DEXr { get; private set; }
        public int INTr { get; private set; }
        public int LUKr { get; private set; }

        public int MaxHPr { get; private set; }
        public int MaxMPr { get; private set; }

        public int PADr { get; private set; }
        public int PDDr { get; private set; }
        public int MADr { get; private set; }
        public int MDDr { get; private set; }
        public int ACCr { get; private set; }
        public int EVAr { get; private set; }

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

        public async Task Calculate()
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
            Craft = INT + DEX + LUK;
            Speed = 100;
            Jump = 100;

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

            var equipped = character.Inventories[ItemInventoryType.Equip].Items
                .Where(kv => kv.Key < 0)
                .Where(kv => kv.Value is ItemSlotEquip)
                .Select(kv => (kv.Key, (ItemSlotEquip)kv.Value))
                .ToList();
            var equippedID = equipped.Select(e => e.Item2.TemplateID).ToList();
            var setIDs = new List<int>();

            await Task.WhenAll(equipped.Select(async kv =>
            {
                var (pos, i) = kv;

                STR += i.STR;
                DEX += i.DEX;
                INT += i.INT;
                LUK += i.LUK;

                MaxHP += i.MaxHP;
                MaxMP += i.MaxMP;

                if (
                    pos != -30 &&
                    pos != -38 &&
                    pos != -31 &&
                    pos != -39 &&
                    pos != -32 &&
                    pos != -40 &&
                    (i.TemplateID / 10000 == 190 || pos != -18 && pos != -19 && pos != -20))
                {
                    PAD += i.PAD;
                    PDD += i.PDD;
                    MAD += i.MAD;
                    MDD += i.MDD;
                    ACC += i.ACC;
                    EVA += i.EVA;
                    Craft += i.Craft;
                    Speed += i.Speed;
                    Jump += i.Jump;
                }

                var template = await _itemTemplates.Retrieve(i.TemplateID);
                if (template is not ItemEquipTemplate equipTemplate) return;
                if (equipTemplate.SetItemID > 0) setIDs.Add(equipTemplate.SetItemID);
                // TODO: and not Dragon or Mechanic

                MaxHPr += equipTemplate.IncMaxHPr;
                MaxMPr += equipTemplate.IncMaxMPr;

                var itemReqLevel = equipTemplate.ReqLevel;
                var itemOptionLevel = (itemReqLevel - 1) / 10;

                itemOptionLevel = Math.Max(itemOptionLevel, 1);

                await ApplyItemOption(i.Option1, itemOptionLevel);
                await ApplyItemOption(i.Option2, itemOptionLevel);
                await ApplyItemOption(i.Option3, itemOptionLevel);
            }));

            await Task.WhenAll(setIDs.Distinct().Select(s => ApplyItemSetEffect(s, setIDs.Count(i => i == s))));

            var skills = _user.Character.SkillRecord;

            await Task.WhenAll(skills.Select(async kv =>
            {
                var skillTemplate = await _skillTemplates.Retrieve(kv.Key);

                if (skillTemplate == null) return;
                if (!skillTemplate.IsPSD) return;
                if (!skillTemplate.LevelData.ContainsKey(kv.Value.Level)) return;

                var skillLevelTemplate = skillTemplate.LevelData[kv.Value.Level];

                // TODO: more psd handling

                MaxHPr += skillLevelTemplate.MHPr;
                MaxMPr += skillLevelTemplate.MMPr;
            }));

            if (character.Job % 1000 / 100 == 5 && !character.Inventories[ItemInventoryType.Equip].Items.ContainsKey((short)BodyPart.Weapon))
                PAD += (int)(character.Level > 30 ? 31d : character.Level * 0.7 + 10d);

            STR += (int)(STR * (STRr / 100d));
            DEX += (int)(DEX * (DEXr / 100d));
            INT += (int)(INT * (INTr / 100d));
            LUK += (int)(LUK * (LUKr / 100d));

            PAD += (int)(PAD * (PADr / 100d));
            PDD += (int)(PDD * (PDDr / 100d));
            MAD += (int)(MAD * (MADr / 100d));
            MDD += (int)(MDD * (MDDr / 100d));
            ACC += (int)(ACC * (ACCr / 100d));
            EVA += (int)(EVA * (EVAr / 100d));

            MaxHP += (int)(MaxHP * (MaxHPr / 100d));
            MaxMP += (int)(MaxMP * (MaxMPr / 100d));

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
        }

        public async Task ApplyItemOption(int itemOptionID, int level)
        {
            if (itemOptionID <= 0) return;

            var optionData = await _optionTemplates.Retrieve(itemOptionID);

            if (optionData == null) return;
            if (!optionData.LevelData.ContainsKey(level)) return;

            var levelData = optionData.LevelData[level];

            STR += levelData.IncSTR;
            DEX += levelData.IncDEX;
            INT += levelData.IncINT;
            LUK += levelData.IncLUK;
            MaxHP += levelData.IncMaxHP;
            MaxMP += levelData.IncMaxMP;

            STRr += levelData.IncSTRr;
            DEXr += levelData.IncDEXr;
            INTr += levelData.IncINTr;
            LUKr += levelData.IncLUKr;
            MaxHPr += levelData.IncMaxHPr;
            MaxMPr += levelData.IncMaxMPr;

            PAD += levelData.IncPAD;
            PDD += levelData.IncPDD;
            MAD += levelData.IncMAD;
            MDD += levelData.IncMDD;
            ACC += levelData.IncACC;
            EVA += levelData.IncEVA;
            Speed += levelData.IncSpeed;
            Jump += levelData.IncJump;

            PADr += levelData.IncPADr;
            PDDr += levelData.IncPDDr;
            MADr += levelData.IncMADr;
            MDDr += levelData.IncMDDr;
            ACCr += levelData.IncACCr;
            EVAr += levelData.IncEVAr;
        }

        public async Task ApplyItemSetEffect(int setEffectID, int equippedCount)
        {
            if (setEffectID <= 0) return;

            var setData = await _setTemplates.Retrieve(setEffectID);

            if (setData == null) return;

            for (var count = 1; count <= equippedCount; count++)
            {
                if (!setData.Effect.ContainsKey(count)) return;

                var effectData = setData.Effect[count];

                STR += effectData.IncSTR;
                DEX += effectData.IncDEX;
                INT += effectData.IncINT;
                LUK += effectData.IncLUK;

                MaxHP += effectData.IncMaxHP;
                MaxMP += effectData.IncMaxMP;

                PAD += effectData.IncPAD;
                PDD += effectData.IncPDD;
                MAD += effectData.IncMAD;
                MDD += effectData.IncMDD;
                ACC += effectData.IncACC;
                EVA += effectData.IncEVA;
                Craft += effectData.IncCraft;
                Speed += effectData.IncSpeed;
                Jump += effectData.IncJump;
            }
        }
    }
}
