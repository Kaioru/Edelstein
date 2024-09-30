using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Consume;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

namespace Edelstein.Protocol.Gameplay.Game.Items.Consume;

public interface IStatChangeItemUseManager : IItemUseManager<IStatChangeItemUseManagerContext, UserStatChangeItemUseRequest, IItemStatChangeTemplate>;
