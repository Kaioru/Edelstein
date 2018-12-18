using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Item.Option;
using Edelstein.Provider.Templates.Item.Set;
using MoreLinq;

namespace Edelstein.Service.Game.Fields.User.Stats
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

        public int CompletedSetItemID { get; set; }
        public BasicStatRateOption Option { get; set; }

        public BasicStat(FieldUser user)
        {
            _user = user;
            Option = new BasicStatRateOption();
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

            Option.STRr = 0;
            Option.DEXr = 0;
            Option.INTr = 0;
            Option.LUKr = 0;

            Option.MaxHPr = 0;
            Option.MaxMPr = 0;

            var templates = _user.Socket.WvsGame.TemplateManager;
            var equipped = character.GetInventory(ItemInventoryType.Equip).Items
                .OfType<ItemSlotEquip>()
                .Where(i => i.Position < 0)
                .ToList();
            var setItemID = new List<int>();
            
            var incMaxHPr = 0;
            var incMaxMPr = 0;

            equipped.ForEach(i =>
            {
                STR += i.STR;
                DEX += i.DEX;
                INT += i.INT;
                LUK += i.LUK;

                MaxHP += i.MaxHP;
                MaxMP += i.MaxMP;

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
                    
                    STR += effect.IncSTR;
                    DEX += effect.IncDEX;
                    INT += effect.IncINT;
                    LUK += effect.IncLUK;
                    
                    MaxHP += effect.IncMaxHP;
                    MaxMP += effect.IncMaxMP;
                }

                if (count >= info.SetCompleteCount) CompletedSetItemID = s;
            });

            STR += (int) (STR * (Option.STRr / 100d));
            DEX += (int) (DEX * (Option.DEXr / 100d));
            INT += (int) (INT * (Option.INTr / 100d));
            LUK += (int) (LUK * (Option.LUKr / 100d));

            var forced = _user.ForcedStat;

            if (forced.STR > 0) STR = forced.STR;
            if (forced.DEX > 0) STR = forced.DEX;
            if (forced.INT > 0) STR = forced.INT;
            if (forced.LUK > 0) STR = forced.LUK;
            
            MaxHP += (int) (MaxHP * ((Option.MaxHPr + incMaxHPr) / 100d));
            MaxMP += (int) (MaxMP * ((Option.MaxMPr + incMaxMPr) / 100d));

            MaxHP = Math.Min(MaxHP, 99999);
            MaxMP = Math.Min(MaxMP, 99999);
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

            Option.STRr += data.IncSTRr;
            Option.DEXr += data.IncDEXr;
            Option.INTr += data.IncINTr;
            Option.LUKr += data.IncLUKr;
            Option.MaxHPr += data.IncMaxHPr;
            Option.MaxMPr += data.IncMaxMPr;
        }
    }
}