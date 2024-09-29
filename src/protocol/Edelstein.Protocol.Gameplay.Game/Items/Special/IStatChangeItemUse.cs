using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;

namespace Edelstein.Protocol.Gameplay.Game.Items.Special;

public interface IStatChangeItemUse : IItemUse<IItemStatChangeTemplate>
{
    int? HP { get; set; }
    int? MP { get; set; }
}
