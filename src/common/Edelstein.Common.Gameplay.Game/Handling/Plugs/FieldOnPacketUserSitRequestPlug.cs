using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserSitRequestPlug : IPipelinePlug<FieldOnPacketUserSitRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserSitRequest message)
    {
        if (message.User.ActivePortableChair > 0)
            await message.User.SetActivePortableChair(0);
        await message.User.SetActiveChair(message.ChairID == -1 ? null : message.ChairID);
    }
}
