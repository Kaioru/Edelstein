using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Utilities.Calculators;

namespace Edelstein.Protocol.Gameplay.Game.Items.Options;

public interface IItemOptionsCalculator : ICalculator<ItemSlotEquip, IItemOptionsCalculatorContext, IItemOptions>;
