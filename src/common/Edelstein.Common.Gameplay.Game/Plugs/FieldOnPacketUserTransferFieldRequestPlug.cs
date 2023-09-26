using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserTransferFieldRequestPlug : IPipelinePlug<FieldOnPacketUserTransferFieldRequest>
{
    private readonly IFieldManager _fieldManager;
    
    public FieldOnPacketUserTransferFieldRequestPlug(IFieldManager fieldManager) => _fieldManager = fieldManager;
    
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserTransferFieldRequest message)
    {
        if (message.FieldID != -1/* && message.User.Account.GradeCode.HasFlag(AccountGradeCode.AdminLevel1)*/)
        {
            var target = await _fieldManager.Retrieve(message.FieldID);
            if (target == null) return;
            await target.Enter(message.User);
            return;
        }

        var portal = message.User.Field?.Template.Portals.Objects
            .FirstOrDefault(o => o.Name == message.PortalID);
        if (portal == null) return;
        var field = await _fieldManager.Retrieve(portal.ToMap);
        if (field == null) return;
        
        if (portal.ToName != null)
            await field.Enter(message.User, portal.ToName);
        else 
            await field.Enter(message.User);
    }
}
