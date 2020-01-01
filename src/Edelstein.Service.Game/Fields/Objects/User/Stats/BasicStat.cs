using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Etc.ItemOption;
using Edelstein.Core.Templates.Etc.SetItemInfo;
using Edelstein.Core.Templates.Items;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Provider;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.Objects.User.Stats
{
    public class BasicStat
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

        public int CompletedSetItemID { get; private set; }

        private readonly FieldUser _user;

        public BasicStat(FieldUser user)
            => _user = user;

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
            PDD = (int) (INT * 0.4 + 0.5 * LUK + DEX * 0.5 + STR * 1.2);
            MAD = 0;
            MDD = (int) (STR * 0.4 + 0.5 * DEX + LUK * 0.5 + INT * 1.2);
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

            CompletedSetItemID = 0;

            var templates = _user.Service.TemplateManager;
            var equipped = character.Inventories[ItemInventoryType.Equip].Items
                .Where(kv => kv.Key < 0)
                .Where(kv => kv.Value is ItemSlotEquip)
                .Select(kv => (kv.Key, (ItemSlotEquip) kv.Value))
                .ToList();
            var setItemID = new List<int>();

            equipped.ForEach(kv =>
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

                var template = templates.Get<ItemTemplate>(i.TemplateID);
                if (!(template is ItemEquipTemplate equipTemplate)) return;
                if (equipTemplate.SetItemID > 0) setItemID.Add(equipTemplate.SetItemID);
                // TODO: and not Dragon or Mechanic

                var itemReqLevel = equipTemplate.ReqLevel;
                var itemOptionLevel = (itemReqLevel - 1) / 10;

                itemOptionLevel = Math.Max(itemOptionLevel, 1);

                MaxMPr += equipTemplate.IncMaxHPr;
                MaxMPr += equipTemplate.IncMaxMPr;

                ApplyItemOption(templates, i.Option1, itemOptionLevel);
                ApplyItemOption(templates, i.Option2, itemOptionLevel);
                ApplyItemOption(templates, i.Option3, itemOptionLevel);
            });

            var equippedID = equipped.Select(e => e.Item2.TemplateID).ToList();

            setItemID.Distinct().ForEach(s =>
            {
                var info = templates.Get<SetItemInfoTemplate>(s);
                var count = info.TemplateID.Count(id => equippedID.Contains(id));

                if (count <= 0) return;

                for (var i = 1; i <= count; i++)
                {
                    if (!info.Effect.ContainsKey(i)) continue;
                    var effect = info.Effect[i];

                    STR += effect.IncSTR;
                    DEX += effect.IncDEX;
                    INT += effect.IncINT;
                    LUK += effect.IncLUK;

                    MaxHP += effect.IncMaxHP;
                    MaxMP += effect.IncMaxMP;

                    PAD += effect.IncPAD;
                    PDD += effect.IncPDD;
                    MAD += effect.IncMAD;
                    MDD += effect.IncMDD;
                    ACC += effect.IncACC;
                    EVA += effect.IncEVA;
                    Craft += effect.IncCraft;
                    Speed += effect.IncSpeed;
                    Jump += effect.IncJump;
                }

                if (count >= info.SetCompleteCount) CompletedSetItemID = s;
            });

            if (character.Job % 1000 / 100 == 5)
            {
                if (!character.Inventories[ItemInventoryType.Equip].Items.ContainsKey(-11))
                {
                    var level = character.Level;
                    PAD += (int) (level > 30 ? 31d : level * 0.7 + 10d);
                }
            }

            STR += (int) (STR * (STRr / 100d));
            DEX += (int) (DEX * (DEXr / 100d));
            INT += (int) (INT * (INTr / 100d));
            LUK += (int) (LUK * (LUKr / 100d));

            PAD += (int) (PAD * (PADr / 100d));
            PDD += (int) (PDD * (PDDr / 100d));
            MAD += (int) (MAD * (MADr / 100d));
            MDD += (int) (MDD * (MDDr / 100d));
            ACC += (int) (ACC * (ACCr / 100d));
            EVA += (int) (EVA * (EVAr / 100d));

            MaxHP += (int) (MaxHP * (MaxHPr / 100d));
            MaxMP += (int) (MaxMP * (MaxMPr / 100d));
            
            var forced = _user.ForcedStat;

            if (forced.STR.HasValue) STR = forced.STR.Value;
            if (forced.DEX.HasValue) DEX = forced.DEX.Value;
            if (forced.INT.HasValue) INT = forced.INT.Value;
            if (forced.LUK.HasValue) LUK = forced.LUK.Value;

            if (forced.PAD.HasValue) PAD = forced.PAD.Value;
            if (forced.PDD.HasValue) PDD = forced.PDD.Value;
            if (forced.MAD.HasValue) MAD = forced.MAD.Value;
            if (forced.MDD.HasValue) MDD = forced.MDD.Value;
            if (forced.ACC.HasValue) ACC = forced.ACC.Value;
            if (forced.EVA.HasValue) EVA = forced.EVA.Value;

            if (forced.Speed.HasValue) Speed = forced.Speed.Value;
            if (forced.Jump.HasValue) Jump = forced.Jump.Value;

            MaxHP = Math.Min(MaxHP, 99999);
            MaxMP = Math.Min(MaxMP, 99999);

            PAD = Math.Min(PAD, 29999);
            PDD = Math.Min(PDD, 30000);
            MAD = Math.Min(MAD, 29999);
            MDD = Math.Min(MDD, 30000);
            ACC = Math.Min(ACC, 9999);
            EVA = Math.Min(EVA, 9999);
            Speed = Math.Min(Math.Max(Speed, 100), forced.SpeedMax ?? 140);
            Jump = Math.Min(Math.Max(Jump, 100), 123);
        }

        private void ApplyItemOption(IDataTemplateManager templates, int itemOptionID, int level)
        {
            if (itemOptionID <= 0) return;
            var option = templates.Get<ItemOptionTemplate>(itemOptionID);
            var data = option.LevelData[level];

            STR += data.IncSTR;
            DEX += data.IncDEX;
            INT += data.IncINT;
            LUK += data.IncLUK;
            MaxHP += data.IncMaxHP;
            MaxMP += data.IncMaxMP;

            STRr += data.IncSTRr;
            DEXr += data.IncDEXr;
            INTr += data.IncINTr;
            LUKr += data.IncLUKr;
            MaxHPr += data.IncMaxHPr;
            MaxMPr += data.IncMaxMPr;

            PAD += data.IncPAD;
            PDD += data.IncPDD;
            MAD += data.IncMAD;
            MDD += data.IncMDD;
            ACC += data.IncACC;
            EVA += data.IncEVA;
            Speed += data.IncSpeed;
            Jump += data.IncJump;

            PADr += data.IncPADr;
            PDDr += data.IncPDDr;
            MADr += data.IncMADr;
            MDDr += data.IncMDDr;
            ACCr += data.IncACCr;
            EVAr += data.IncEVAr;
        }
    }
}