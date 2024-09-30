using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Utilities.Pipelines;
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

            var grade = (ItemOptionGrade)input.Grade;

            if (grade == ItemOptionGrade.Normal)
                grade = ItemOptionGrade.Rare;
            if (grade == ItemOptionGrade.Rare && random.NextDouble() < context.GradeIncRateEpic)
                grade = ItemOptionGrade.Epic;
            if (grade == ItemOptionGrade.Epic && random.NextDouble() < context.GradeIncRateUnique)
                grade = ItemOptionGrade.Unique;

            var option1Grade = grade;
            var option2Grade = (ItemOptionGrade)Math.Max(
                (int)ItemOptionGrade.Rare,
                (int)grade - (random.NextDouble() < context.Option2IncRate ? 0 : 1)
            );
            var option3Grade = (ItemOptionGrade)Math.Max(
                (int)ItemOptionGrade.Rare,
                (int)grade - (random.NextDouble() < context.Option3IncRate ? 0 : 1)
            );
            
            var options = await itemOptions.RetrieveAll();

            // TODO filters
            
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
