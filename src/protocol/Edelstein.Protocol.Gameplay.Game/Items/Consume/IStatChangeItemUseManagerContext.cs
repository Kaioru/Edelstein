using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

namespace Edelstein.Protocol.Gameplay.Game.Items.Consume;

public interface IStatChangeItemUseManagerContext : IItemUseManagerContext<IItemStatChangeTemplate, UserStatChangeItemUseRequest>
{
    int? HP { get; set; }
    int? MP { get; set; }
    int? HPr { get; set; }
    int? MPr { get; set; }
}
