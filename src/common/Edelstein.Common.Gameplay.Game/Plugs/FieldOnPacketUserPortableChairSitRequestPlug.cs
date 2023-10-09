using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserPortableChairSitRequestPlug : IPipelinePlug<FieldOnPacketUserPortableChairSitRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserPortableChairSitRequest message)
    {
        if (!message.User.StageUser.Context.Managers.Inventory.HasItem(
                message.User.Character.Inventories, 
                message.TemplateID))
            return;

        if (message.User.ActivePortableChair > 0)
            return;
        
        await message.User.SetActivePortableChair(message.TemplateID);
        await message.User.ModifyStats(exclRequest: true);
    }
}
