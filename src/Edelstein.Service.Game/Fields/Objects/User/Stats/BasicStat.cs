using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Item.Option;
using Edelstein.Provider.Templates.Item.Set;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.Objects.User.Stats
{
    public class BasicStat
    {
        private FieldUser _user;

        public byte Gender { get; set; }
        public byte Level { get; set; }
        public short Job { get; set; }

        public int STR { get; set; }
        public int DEX { get; set; }
        public int INT { get; set; }
        public int LUK { get; set; }

        public int POP { get; set; }

        public int MaxHP { get; set; }
        public int MaxMP { get; set; }

        public int PAD { get; set; }
        public int PDD { get; set; }
        public int MAD { get; set; }
        public int MDD { get; set; }
        public int ACC { get; set; }
        public int EVA { get; set; }
        public int Craft { get; set; }
        public int Speed { get; set; }
        public int Jump { get; set; }

        public int CompletedSetItemID { get; set; }
        public BasicStatRateOption RateOption { get; set; }

        public BasicStat(FieldUser user)
        {
            _user = user;
            RateOption = new BasicStatRateOption();
        }

        public void Calculate()
        {
            var character = _user.Character;
            Gender = character.Gender;
            Level = character.Level;
            Job = character.Job;

            STR = character.STR;
            DEX = character.DEX;
            INT = character.INT;
            LUK = character.LUK;

            POP = character.POP;

            MaxHP = character.MaxHP;
            MaxMP = character.MaxMP;

            PAD = 0;
            MAD = 0;
            Craft = INT + DEX + LUK;
            Speed = 100;
            Jump = 100;

            CompletedSetItemID = 0;

            RateOption.STRr = 0;
            RateOption.DEXr = 0;
            RateOption.INTr = 0;
            RateOption.LUKr = 0;

            RateOption.MaxHPr = 0;
            RateOption.MaxMPr = 0;

            RateOption.PADr = 0;
            RateOption.PDDr = 0;
            RateOption.MADr = 0;
            RateOption.MDDr = 0;
            RateOption.ACCr = 0;
            RateOption.EVAr = 0;

            var templates = _user.Service.TemplateManager;
            var equipped = character.Inventories[ItemInventoryType.Equip].Items
                .Where(kv => kv.Key < 0)
                .Where(kv => kv.Value is ItemSlotEquip)
                .Select(kv => (kv.Key, (ItemSlotEquip) kv.Value))
                .ToList();
            var setItemID = new List<int>();

            var incMaxHPr = 0;
            var incMaxMPr = 0;

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

                incMaxHPr += equipTemplate.IncMaxHPr;
                incMaxMPr += equipTemplate.IncMaxMPr;

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

            STR += (int) (STR * (RateOption.STRr / 100d));
            DEX += (int) (DEX * (RateOption.DEXr / 100d));
            INT += (int) (INT * (RateOption.INTr / 100d));
            LUK += (int) (LUK * (RateOption.LUKr / 100d));

            PAD += (int) (PAD * (RateOption.PADr / 100d));
            PDD += (int) (PDD * (RateOption.PDDr / 100d));
            MAD += (int) (MAD * (RateOption.MADr / 100d));
            MDD += (int) (MDD * (RateOption.MDDr / 100d));
            ACC += (int) (ACC * (RateOption.ACCr / 100d));
            EVA += (int) (EVA * (RateOption.EVAr / 100d));


            var forced = _user.ForcedStat;

            if (forced.STR > 0) STR = forced.STR;
            if (forced.DEX > 0) DEX = forced.DEX;
            if (forced.INT > 0) INT = forced.INT;
            if (forced.LUK > 0) LUK = forced.LUK;

            if (forced.PAD > 0) PAD = forced.PAD;
            if (forced.PDD > 0) PDD = forced.PDD;
            if (forced.MAD > 0) MAD = forced.MAD;
            if (forced.MDD > 0) MDD = forced.MDD;
            if (forced.ACC > 0) ACC = forced.ACC;
            if (forced.EVA > 0) EVA = forced.EVA;
            if (forced.Speed > 0) Speed = forced.Speed;
            if (forced.Jump > 0) Jump = forced.Jump;

            MaxHP += (int) (MaxHP * ((RateOption.MaxHPr + incMaxHPr) / 100d));
            MaxMP += (int) (MaxMP * ((RateOption.MaxMPr + incMaxMPr) / 100d));

            MaxHP = Math.Min(MaxHP, 99999);
            MaxMP = Math.Min(MaxMP, 99999);

            PAD = Math.Min(PAD, 29999);
            PDD = Math.Min(PDD, 30000);
            MAD = Math.Min(MAD, 29999);
            MDD = Math.Min(MDD, 30000);
            ACC = Math.Min(ACC, 9999);
            EVA = Math.Min(EVA, 9999);
            Speed = Math.Min(
                Math.Max(Speed, 100),
                forced.SpeedMax > 0
                    ? forced.SpeedMax
                    : 140
            );
            Jump = Math.Min(Math.Max(Jump, 100), 123);
        }

        private void ApplyItemOption(ITemplateManager templates, int itemOptionID, int level)
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

            RateOption.STRr += data.IncSTRr;
            RateOption.DEXr += data.IncDEXr;
            RateOption.INTr += data.IncINTr;
            RateOption.LUKr += data.IncLUKr;
            RateOption.MaxHPr += data.IncMaxHPr;
            RateOption.MaxMPr += data.IncMaxMPr;

            PAD += data.IncPAD;
            PDD += data.IncPDD;
            MAD += data.IncMAD;
            MDD += data.IncMDD;
            ACC += data.IncACC;
            EVA += data.IncEVA;
            Speed += data.IncSpeed;
            Jump += data.IncJump;

            RateOption.PADr += data.IncPADr;
            RateOption.PDDr += data.IncPDDr;
            RateOption.MADr += data.IncMADr;
            RateOption.MDDr += data.IncMDDr;
            RateOption.ACCr += data.IncACCr;
            RateOption.EVAr += data.IncEVAr;
        }
    }
}