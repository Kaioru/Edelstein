using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Stats;

public class FieldUserStatsCalculator : IFieldUserStatsCalculator
{
    private readonly ITemplateManager<IItemTemplate> _itemTemplates;
    
    public FieldUserStatsCalculator(ITemplateManager<IItemTemplate> itemTemplates)
    {
        _itemTemplates = itemTemplates;
    }

    public async Task<IFieldUserStats> Calculate(IFieldUser user)
    {
        var character = user.Character;
        var str = (int)character.STR;
        var dex = (int)character.DEX;
        var @int = (int)character.INT;
        var luk = (int)character.LUK;
        var maxHP = character.MaxHP;
        var maxMP = character.MaxMP;

        var pad = 0;
        var pdd = 0;
        var mad = 0;
        var mdd = 0;

        var acc = 0;
        var eva = 0;

        var craft = @int + dex + luk;
        var speed = 100;
        var jump = 100;
        
        var strR = 0;
        var dexR = 0;
        var intR = 0;
        var lukR = 0;
        var maxHPr = 0;
        var maxMPr = 0;
        
        var equippedItems = character.Inventories[ItemInventoryType.Equip]?.Items
            .Where(kv => kv.Key < 0)
            .Where(kv => kv.Value is ItemSlotEquip)
            .Select(kv => (kv.Key, (ItemSlotEquip)kv.Value))
            .ToList() ?? new List<(short Key, ItemSlotEquip)>();

        foreach (var kv in equippedItems)
        {
            var (pos, item) = kv;
            var template = await _itemTemplates.Retrieve(item.ID);

            if (template is not IItemEquipTemplate equipTemplate) continue;
            
            str += item.STR;
            dex += item.DEX;
            @int += item.INT;
            luk += item.LUK;
            maxHP += item.MaxHP;
            maxMP += item.MaxMP;

            if (
                pos != -(int)BodyPart.PetWear2 &&
                pos != -(int)BodyPart.PetWear3 &&
                pos != -(int)BodyPart.PetRingLabel2 &&
                pos != -(int)BodyPart.PetRingLabel3 &&
                pos != -(int)BodyPart.PetRingQuote2 &&
                pos != -(int)BodyPart.PetRingQuote3 &&
                (
                    item.ID / 10000 == 190 || 
                    pos != -(int)BodyPart.TamingMob && 
                    pos != -(int)BodyPart.Saddle && 
                    pos != -(int)BodyPart.MobEquip
                )
            )
            {
                pad += item.PAD;
                pdd += item.PDD;
                mad += item.MAD;
                mdd += item.MDD;
                acc += item.ACC;
                eva += item.EVA;
                craft += item.Craft;
                speed += item.Speed;
                jump += item.Jump;
            }

            maxHPr += equipTemplate.IncMaxHPr;
            maxMPr += equipTemplate.IncMaxMPr;

            // TODO: and not Dragon or Mechanic
            // TODO: item options
        }

        // TODO: item sets
        
        str += (int)(str * (strR / 100d));
        dex += (int)(dex * (dexR / 100d));
        @int += (int)(@int * (intR / 100d));
        luk += (int)(luk * (lukR / 100d));
        maxHP += (int)(maxHP * (maxHPr / 100d));
        maxMP += (int)(maxMP * (maxMPr / 100d));
        
        maxHP = Math.Min(maxHP, 99999);
        maxMP = Math.Min(maxMP, 99999);

        pad = Math.Min(pad, 29999);
        pdd = Math.Min(pdd, 30000);
        mad = Math.Min(mad, 29999);
        mdd = Math.Min(mdd, 30000);
        speed = Math.Min(Math.Max(speed, 100), 140);
        jump = Math.Min(Math.Max(jump, 100), 123);
        
        return new FieldUserStats
        {
            Level = character.Level,
            
            STR = str,
            DEX = dex,
            INT = @int,
            LUK = luk,
            
            MaxHP = maxHP,
            MaxMP = maxMP,
            
            ACC = acc,
            EVA = eva,
            
            PAD = pad,
            PDD = pdd,
            MAD = mad,
            MDD = mdd,
            
            Craft = craft,
            Speed = speed,
            Jump = jump,
            
            STRr = strR,
            DEXr = dexR,
            INTr = intR,
            LUKr = lukR,
            MaxHPr = maxHPr,
            MaxMPr = maxMPr
        };
    }
}
