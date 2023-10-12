using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketDropPickupRequestPlug : IPipelinePlug<FieldOnPacketDropPickupRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketDropPickupRequest message)
    {
        await message.Drop.PickUp(message.User);
        await message.User.ModifyInventory(exclRequest: true);
    }
}
