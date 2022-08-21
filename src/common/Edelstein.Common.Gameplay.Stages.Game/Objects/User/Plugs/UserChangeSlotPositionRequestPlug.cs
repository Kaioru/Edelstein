using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserChangeSlotPositionRequestPlug : IPipelinePlug<IUserChangeSlotPositionRequest>
{
    public async Task Handle(IPipelineContext ctx, IUserChangeSlotPositionRequest message)
    {
        if (message.To == 0)
        {
            await message.User.ModifyInventory(exclRequest: true);
            return;
        }

        await message.User.ModifyInventory(
            i => i[message.Type]?.MoveSlot(message.From, message.To),
            true
        );
    }
}
