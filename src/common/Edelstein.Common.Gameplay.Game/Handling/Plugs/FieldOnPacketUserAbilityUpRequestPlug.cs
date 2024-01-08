using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Plugs;

public class FieldOnPacketUserAbilityUpRequestPlug : IPipelinePlug<FieldOnPacketUserAbilityUpRequest>
{
    private readonly FieldOnPacketUserAbilityMassUpRequestPlug _plug;
    
    public FieldOnPacketUserAbilityUpRequestPlug(FieldOnPacketUserAbilityMassUpRequestPlug plug)
        => _plug = plug;

    public Task Handle(IPipelineContext ctx, FieldOnPacketUserAbilityUpRequest message)
        => _plug.Handle(ctx, new FieldOnPacketUserAbilityMassUpRequest(
            message.User,
            new Dictionary<ModifyStatType, int>
            {
                [message.Type] = 1
            }
        ));
}
