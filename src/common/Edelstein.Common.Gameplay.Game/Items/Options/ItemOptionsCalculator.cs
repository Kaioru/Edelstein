using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Utilities.Pipelines;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Game.Items.Options;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Options;

public class ItemOptionsCalculator(
    ITemplateManager<IItemTemplate> items,
    ITemplateManager<IItemOptionTemplate> itemOptions
) :
    Pipeline<IItemOptionsCalculatorContext>,
    IItemOptionsCalculator
{
    private const double GradeIncRateEpic = 0.06;
    private const double GradeIncRateUnique = 0.018;

    private const double Option2SetRate = 0.10;
    private const double Option2IncRate = 0.10;
    private const double Option3SetRate = 0.01;
    private const double Option3IncRate = 0.01;

    public async Task<IItemOptions> Calculate(ItemSlotEquip input)
    {
        if (await items.Retrieve(input.TemplateID) is IItemEquipTemplate template)
        {
            var random = new Random();
            var context = new ItemOptionsCalculatorContext(input, template)
            {
                GradeIncRateEpic = GradeIncRateEpic,
                GradeIncRateUnique = GradeIncRateUnique,

                Option2SetRate = Option2SetRate,
                Option2IncRate = Option2IncRate,
                Option3SetRate = Option3SetRate,
                Option3IncRate = Option3IncRate,
            };

            await Process(context);

            var grade = (ItemOptionGrade)(input.Grade & 0x3);

            if (grade == ItemOptionGrade.Normal)
                grade = ItemOptionGrade.Rare;
            if (grade == ItemOptionGrade.Rare && random.NextDouble() < context.GradeIncRateEpic)
                grade = ItemOptionGrade.Epic;
            if (grade == ItemOptionGrade.Epic && random.NextDouble() < context.GradeIncRateUnique)
                grade = ItemOptionGrade.Unique;

            var option1Grade = grade;
            var option2Grade = (ItemOptionGrade)((int)grade - (random.NextDouble() < context.Option2IncRate ? 0 : 1));
            var option3Grade = (ItemOptionGrade)((int)grade - (random.NextDouble() < context.Option3IncRate ? 0 : 1));

            var bodyParts = template.ID.GetBodyParts();
            var options = (await itemOptions.RetrieveAll())
                .Where(o => o.Type switch
                {
                    ItemOptionType.AnyEquip => true,
                    ItemOptionType.AnyWeapon => bodyParts.Contains(BodyPart.Weapon),
                    ItemOptionType.AnyArmorOrAccessory => bodyParts.Any(bp => bp.IsArmor() || bp.IsAccessory()),
                    ItemOptionType.AnyArmorOrShield => bodyParts.Any(bp => bp.IsArmor() || bp == BodyPart.Shield),
                    ItemOptionType.AnyAccessory => bodyParts.Any(bp => bp.IsAccessory()),
                    ItemOptionType.AnyCap  => bodyParts.Any(bp => bp == BodyPart.Cap),
                    ItemOptionType.AnyCoat => bodyParts.Any(bp => bp == BodyPart.Clothes),
                    ItemOptionType.AnyPants => bodyParts.Any(bp => bp == BodyPart.Pants),
                    ItemOptionType.AnyGloves => bodyParts.Any(bp => bp == BodyPart.Gloves),
                    ItemOptionType.AnyShoe => bodyParts.Any(bp => bp == BodyPart.Shoes),
                    ItemOptionType.AnyAccessoryNotBelt => bodyParts.Any(bp => bp.IsAccessory()) && 
                                                          bodyParts.All(bp => bp != BodyPart.Belt),
                    _ => false
                })
                .ToImmutableList();
            
            var option1 = (short)random.GetItems(options
                .Where(o => o.Grade == option1Grade)
                .ToArray(), 1).First().ID;
            var option2 = input.Option2;
            var option3 = input.Option3;

            if (option2 > 0 || random.NextDouble() < context.Option2IncRate)
                option2 = (short)random.GetItems(options
                    .Where(o => o.Grade == option2Grade)
                    .ToArray(), 1).First().ID;
            if (option3 > 0 || random.NextDouble() < context.Option3IncRate)
                option3 = (short)random.GetItems(options
                    .Where(o => o.Grade == option3Grade)
                    .ToArray(), 1).First().ID;

            return new ItemOptions(
                grade,
                option1,
                option2,
                option3
            );
        }

        return new ItemOptions(
            (ItemOptionGrade)input.Grade,
            input.Option1,
            input.Option2,
            input.Option3
        );
    }
}
