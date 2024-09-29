using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling;

public abstract class AbstractUserOnPacketUserItemUseRequest<TPacket, TContext, TTemplate>(
    IItemUseManager<TContext, TPacket, TTemplate> manager,
    ItemInventoryType inventory
) : AbstractUserOnPacketInField<TPacket>
    where TContext : IItemUseManagerContext<TTemplate>
    where TPacket : StructuredBasePacket, IItemUseInfo
    where TTemplate : IItemTemplate
{
    protected override Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<TPacket> message)
        => manager.Use(
            message.User,
            inventory,
            message.Packet,
            true
        );
}
