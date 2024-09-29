using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

namespace Edelstein.Protocol.Gameplay.Game.Items.Consume;

public interface IStatChangeItemUseManager : IItemUseManager<IStatChangeItemUseManagerContext, UserStatChangeItemUseRequest, IItemStatChangeTemplate>;
