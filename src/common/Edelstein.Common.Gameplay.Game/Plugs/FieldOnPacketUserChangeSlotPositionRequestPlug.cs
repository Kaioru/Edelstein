using Edelstein.Common.Gameplay.Game.Objects.Drop;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Drop;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserChangeSlotPositionRequestPlug : IPipelinePlug<FieldOnPacketUserChangeSlotPositionRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserChangeSlotPositionRequest message)
    {
        if (message.To == 0)
        {
            IItemSlot? item = null;
            
            await message.User.ModifyInventory(
                i => item = i[message.Type]?.TakeSlot(message.From, message.Count),
                true
            );

            if (item == null) return;

            var foothold = message.User.Field?.Template.Footholds
                .FindBelow(message.User.Position)
                .FirstOrDefault();
            var position = foothold?.Line.AtX(message.User.Position.X);

            if (position == null) return;

            var drop = new FieldDropItem(position, item)
            {
                OwnType = DropOwnType.NoOwn,
                SourceID = message.User.ObjectID ?? 0
            };
            
            await message.User.Field!.Enter(
                drop,
                () => drop.GetEnterFieldPacket(1, message.User.Position)
            );
            return;
        }

        await message.User.ModifyInventory(
            i => i[message.Type]?.MoveSlot(message.From, message.To),
            true
        );
    }
}
