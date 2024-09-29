using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling;

public abstract class AbstractUserOnPacketUserItemUseRequest<TPacket, TContext, TTemplate>(
    IItemUseManager<TContext, TTemplate> manager,
    ItemInventoryType inventory
) : AbstractUserOnPacketInField<TPacket>
    where TPacket : StructuredItemUseRequest
    where TContext : IItemUse<TTemplate>
    where TTemplate : IItemTemplate
{
    protected override Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<TPacket> message)
        => manager.Use(
            message.User,
            inventory,
            message.Packet.Pos,
            true
        );
}
