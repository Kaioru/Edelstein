using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

namespace Edelstein.Protocol.Gameplay.Game.Items.Consume;

public interface IItemOptionUpgradeItemUseManager : IItemUseManager<IItemOptionUpgradeItemUseManagerContext, UserItemOptionUpgradeItemUseRequest, IItemBundleTemplate>;
