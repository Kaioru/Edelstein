using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserShopCloseRequestPlug : IPipelinePlug<FieldOnPacketUserShopCloseRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserShopCloseRequest message)
    {
        await message.User.EndDialogue();
        await message.User.ModifyStats(exclRequest: true);
    }
}
