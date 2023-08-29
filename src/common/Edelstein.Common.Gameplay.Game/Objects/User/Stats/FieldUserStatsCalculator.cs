using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Models.Inventories;
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
        var str = character.STR;
        var dex = character.DEX;
        var @int = character.INT;
        var luk = character.LUK;
        var maxHP = character.MaxHP;
        var maxMP = character.MaxMP;

        var pad = 0;
        var pdd = 0;
        var mad = 0;
        var mdd = 0;

        var craft = @int + dex + luk;
        var speed = 100;
        var jump = 100;
        
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

            if ( // TODO use BodyPart
                pos != -30 &&
                pos != -38 &&
                pos != -31 &&
                pos != -39 &&
                pos != -32 &&
                pos != -40 &&
                (item.ID / 10000 == 190 || pos != -18 && pos != -19 && pos != -20))
            {
                pad += item.PAD;
                pdd += item.PDD;
                mad += item.MAD;
                mdd += item.MDD;
                // ACC += item.ACC;
                // EVA += item.EVA;
                craft += item.Craft;
                speed += item.Speed;
                jump += item.Jump;
            }

            // MaxHPr += equipTemplate.IncMaxHPr;
            // MaxMPr += equipTemplate.IncMaxMPr;

            // TODO: and not Dragon or Mechanic
            // TODO: item options
        }

        // TODO: item sets
        
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
            STR = str,
            DEX = dex,
            INT = @int,
            LUK = luk,
            
            MaxHP = maxHP,
            MaxMP = maxMP,
            
            PAD = pad,
            PDD = pdd,
            MAD = mad,
            MDD = mdd,
            
            Craft = craft,
            Speed = speed,
            Jump = jump
        };
    }
}
