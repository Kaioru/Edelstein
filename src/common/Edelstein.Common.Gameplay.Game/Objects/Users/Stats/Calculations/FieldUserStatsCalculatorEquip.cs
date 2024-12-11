using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats.Calculations;

public class FieldUserStatsCalculatorEquip(
    ITemplateManager<IItemTemplate> items,
    ITemplateManager<IItemOptionTemplate> options
) : IFieldUserStatsCalculatorEntry
{
    public int Priority => FieldUserStatsCalculatorSteps.Equip;
    
    public async Task Handle(IPipelineContext ctx, IFieldUserStatsCalculatorContext stats)
    {
        var equipped = stats.User.Character.Inventories[ItemInventoryType.Equip]?.Items
            .Where(kv => kv.Key < 0)
            .Where(kv => kv.Value is ItemSlotEquip)
            .Select(kv => (kv.Key, (ItemSlotEquip)kv.Value))
            .ToImmutableArray() ?? ImmutableArray<(short Key, ItemSlotEquip)>.Empty;
        
        foreach (var (slot, item) in equipped)
        {
            if (await items.Retrieve(item.TemplateID) is not IItemEquipTemplate template) continue;
            
            stats.STR.IncBase += item.STR;
            stats.DEX.IncBase += item.DEX;
            stats.INT.IncBase += item.INT;
            stats.LUK.IncBase += item.LUK;
            stats.MaxHP.IncBase += item.MaxHP;
            stats.MaxMP.IncBase += item.MaxMP;

            if (slot != -(int)BodyPart.PetWear2 &&
                slot != -(int)BodyPart.PetWear3 &&
                slot != -(int)BodyPart.PetRingLabel2 &&
                slot != -(int)BodyPart.PetRingLabel3 &&
                slot != -(int)BodyPart.PetRingQuote2 &&
                slot != -(int)BodyPart.PetRingQuote3 &&
                (
                    item.TemplateID / 10000 == 190 ||
                    slot != -(int)BodyPart.TamingMob &&
                    slot != -(int)BodyPart.Saddle &&
                    slot != -(int)BodyPart.MobEquip
                )
               )
            {
                stats.PAD.IncBase += item.PAD;
                stats.PDD.IncBase += item.PDD;
                stats.MAD.IncBase += item.MAD;
                stats.MDD.IncBase += item.MDD;
                stats.ACC.IncBase += item.ACC;
                stats.EVA.IncBase += item.EVA;
                stats.Craft.IncBase += item.Craft;
                stats.Speed.IncBase += item.Speed;
                stats.Jump.IncBase += item.Jump;

                stats.MaxHP.IncRate += template.IncMaxHPr;
                stats.MaxMP.IncRate += template.IncMaxMPr;

                var grade = (ItemOptionGrade)(item.Grade & 0x3);
                if ((item.Grade & (int)ItemOptionGradeState.Released) > 0 &&
                    grade is 
                        ItemOptionGrade.Rare or 
                        ItemOptionGrade.Epic or 
                        ItemOptionGrade.Unique)
                {
                    var level = (template.ReqLevel - 1) / 10;

                    level = Math.Max(1, level);
                    level = Math.Min(20, level);

                    await ApplyItemOption(stats, item.Option1, level);
                    await ApplyItemOption(stats, item.Option2, level);
                    await ApplyItemOption(stats, item.Option3, level);
                }
            }
        }
    }

    private async Task ApplyItemOption(IFieldUserStatsCalculatorContext stats, int option, int level)
    {
        var template = await options.Retrieve(option);
        if (template == null) return;
        var templateLevel = await template.Levels.Retrieve(level);
        if (templateLevel == null) return;
        
        stats.STR.IncBase += templateLevel.IncSTR;
        stats.DEX.IncBase += templateLevel.IncDEX;
        stats.LUK.IncBase += templateLevel.IncLUK;
        stats.INT.IncBase += templateLevel.IncINT;
        
        stats.STR.IncRate += templateLevel.IncSTRr;
        stats.DEX.IncRate += templateLevel.IncDEXr;
        stats.LUK.IncRate += templateLevel.IncLUKr;
        stats.INT.IncRate += templateLevel.IncINTr;

        stats.MaxHP.IncBase += templateLevel.IncMaxHP;
        stats.MaxMP.IncBase += templateLevel.IncMaxMP;
        
        stats.MaxHP.IncRate += templateLevel.IncMaxHPr;
        stats.MaxMP.IncRate += templateLevel.IncMaxMPr;

        stats.PAD.IncBase += templateLevel.IncPAD;
        stats.PDD.IncBase += templateLevel.IncPDD;
        stats.MAD.IncBase += templateLevel.IncMAD;
        stats.MDD.IncBase += templateLevel.IncMDD;
        stats.ACC.IncBase += templateLevel.IncACC;
        stats.EVA.IncBase += templateLevel.IncEVA;
        
        stats.PAD.IncRate += templateLevel.IncPADr;
        stats.PDD.IncRate += templateLevel.IncPDDr;
        stats.MAD.IncRate += templateLevel.IncMADr;
        stats.MDD.IncRate += templateLevel.IncMDDr;
        stats.ACC.IncRate += templateLevel.IncACCr;
        stats.EVA.IncRate += templateLevel.IncEVAr;

        stats.Speed.IncBase += templateLevel.IncSpeed;
        stats.Jump.IncBase += templateLevel.IncJump;
    }
}
