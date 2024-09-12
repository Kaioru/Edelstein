using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats.Calculations;

public class FieldUserStatsCalculatorEquip(
    ITemplateManager<IItemTemplate> items
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
            }
        }
    }
}
