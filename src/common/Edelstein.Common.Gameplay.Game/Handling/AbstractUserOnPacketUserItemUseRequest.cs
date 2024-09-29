using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling;

public class AbstractUserOnPacketUserItemUseRequest<TPacket, TContext, TTemplate>(
    IItemUseManager<TContext, TTemplate> manager
) : AbstractUserOnPacketInField<TPacket>
    where TPacket : StructuredItemUseRequest
    where TContext : IItemUse<TTemplate>
    where TTemplate : IItemTemplate
{
    protected override Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<TPacket> message)
        => manager.Use(
            message.User,
            ItemInventoryType.Consume,
            message.Packet.Pos,
            true
        );
}
