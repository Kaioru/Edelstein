using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;

namespace Edelstein.Protocol.Gameplay.Game.Items.Consume;

public interface IStatChangeItemUse : IItemUse<IItemStatChangeTemplate>
{
    int? HP { get; set; }
    int? MP { get; set; }
    int? HPr { get; set; }
    int? MPr { get; set; }
}
