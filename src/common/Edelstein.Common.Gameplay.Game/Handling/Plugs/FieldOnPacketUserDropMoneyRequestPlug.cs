using Edelstein.Common.Gameplay.Game.Objects.Drop;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.Drop;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserDropMoneyRequestPlug : IPipelinePlug<FieldOnPacketUserDropMoneyRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserDropMoneyRequest message)
    {
        if (message.Money is < 10 or > 50000) return;
        if (message.User.Character.Money < message.Money) return;
        
        var foothold = message.User.Field?.Template.Footholds
            .FindBelow(new Point2D(
                message.User.Position.X,
                message.User.Position.Y - 100
            ))
            .FirstOrDefault();
        var position = foothold?.Line.AtX(message.User.Position.X);

        if (position == null) return;

        var drop = new FieldDropMoney(position, message.Money)
        {
            OwnType = DropOwnType.NoOwn
        };

        await message.User.ModifyStats(s => s.Money -= message.Money, true);
        await message.User.Field!.Enter(
            drop,
            () => drop.GetEnterFieldPacket(1, message.User.Position)
        );
    }
}
