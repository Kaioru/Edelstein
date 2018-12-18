using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Item.Option;
using Edelstein.Provider.Templates.Item.Set;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.User.Stats
{
    public class SecondaryStat
    {
        private FieldUser _user;

        public int PAD { get; set; }
        public int PDD { get; set; }
        public int MAD { get; set; }
        public int MDD { get; set; }
        public int ACC { get; set; }
        public int EVA { get; set; }
        public int Craft { get; set; }
        public int Speed { get; set; }
        public int Jump { get; set; }

        public SecondaryStatRateOption Option { get; set; }

        public SecondaryStat(FieldUser user)
        {
            _user = user;
            Option = new SecondaryStatRateOption();
        }

        public void Calculate()
        {
            var character = _user.Character;
            var bs = _user.BasicStat;

            PAD = 0;
            PDD = (int) (bs.INT * 0.4d + 0.5d * bs.LUK + bs.DEX * 0.5d + bs.STR * 1.2d);
            MAD = 0;
            MDD = (int) (bs.STR * 0.4d + 0.5d * bs.DEX + bs.LUK * 0.5d + bs.INT * 1.2d);
            ACC = (int) (bs.LUK + bs.DEX * 1.2);
            EVA = bs.DEX + 2 * bs.LUK;
            Craft = bs.INT + bs.DEX + bs.LUK;
            Speed = 100;
            Jump = 100;

            Option.PADr = 0;
            Option.PDDr = 0;
            Option.MADr = 0;
            Option.MDDr = 0;
            Option.ACCr = 0;
            Option.EVAr = 0;

            var templates = _user.Socket.WvsGame.TemplateManager;
            var equipped = character.GetInventory(ItemInventoryType.Equip).Items
                .OfType<ItemSlotEquip>()
                .Where(i => i.Position < 0)
                .ToList();
            var setItemID = new List<int>();

            equipped.ForEach(i =>
            {
                var pos = i.Position;
                if (pos == -30 ||
                    pos == -38 ||
                    pos == -31 ||
                    pos == -39 ||
                    pos == -32 ||
                    pos == -40 ||
                    i.TemplateID / 10000 != 190 &&
                    (
                        pos == -18 ||
                        pos == -19 ||
                        pos == -20
                    )
                ) return;

                PAD += i.PAD;
                PDD += i.PDD;
                MAD += i.MAD;
                MDD += i.MDD;
                ACC += i.ACC;
                EVA += i.EVA;
                Craft += i.Craft;
                Speed += i.Speed;
                Jump += i.Jump;

                var template = templates.Get<ItemTemplate>(i.TemplateID);
                if (!(template is ItemEquipTemplate equipTemplate)) return;
                if (equipTemplate.SetItemID > 0) setItemID.Add(equipTemplate.SetItemID);

                var itemReqLevel = equipTemplate.ReqLevel;
                var itemOptionLevel = (itemReqLevel - 1) / 10;

                itemOptionLevel = Math.Max(itemOptionLevel, 1);

                ApplyItemOption(templates, i.Option1, itemOptionLevel);
                ApplyItemOption(templates, i.Option2, itemOptionLevel);
                ApplyItemOption(templates, i.Option3, itemOptionLevel);
            });

            var equippedID = equipped.Select(e => e.TemplateID).ToList();

            setItemID.Distinct().ForEach(s =>
            {
                var info = templates.Get<SetItemInfoTemplate>(s);
                var count = info.ItemTemplateID.Count(id => equippedID.Contains(id));

                if (count <= 0) return;

                for (var i = 1; i <= count; i++)
                {
                    if (!info.Effect.ContainsKey(i)) continue;
                    var effect = info.Effect[i];

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
            });

            if (character.Job % 1000 / 100 == 5)
            {
                var weapon = character.GetInventory(ItemInventoryType.Equip).Items
                    .OfType<ItemSlotEquip>()
                    .FirstOrDefault(i => i.Position == -11);

                if (weapon == null)
                {
                    var level = character.Level;
                    PAD += (int) (level > 30 ? 31d : level * 0.7 + 10d);
                }
            }

            PAD += (int) (PAD * (Option.PADr / 100d));
            PDD += (int) (PDD * (Option.PDDr / 100d));
            MAD += (int) (MAD * (Option.MADr / 100d));
            MDD += (int) (MDD * (Option.MDDr / 100d));
            ACC += (int) (ACC * (Option.ACCr / 100d));
            EVA += (int) (EVA * (Option.EVAr / 100d));

            var forced = _user.ForcedStat;

            if (forced.PAD > 0) PAD = forced.PAD;
            if (forced.PDD > 0) PDD = forced.PDD;
            if (forced.MAD > 0) MAD = forced.MAD;
            if (forced.MDD > 0) MDD = forced.MDD;
            if (forced.ACC > 0) ACC = forced.ACC;
            if (forced.EVA > 0) EVA = forced.EVA;
            if (forced.Speed > 0) Speed = forced.Speed;
            if (forced.Jump > 0) Jump = forced.Jump;

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

            PAD += data.IncPAD;
            PDD += data.IncPDD;
            MAD += data.IncMAD;
            MDD += data.IncMDD;
            ACC += data.IncACC;
            EVA += data.IncEVA;
            Speed += data.IncSpeed;
            Jump += data.IncJump;

            Option.PADr += data.IncPADr;
            Option.PDDr += data.IncPDDr;
            Option.MADr += data.IncMADr;
            Option.MDDr += data.IncMDDr;
            Option.ACCr += data.IncACCr;
            Option.EVAr += data.IncEVAr;
        }
    }
}