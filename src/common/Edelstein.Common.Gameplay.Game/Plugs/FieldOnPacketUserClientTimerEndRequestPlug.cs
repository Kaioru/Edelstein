using Edelstein.Common.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Plugs;

public class FieldOnPacketUserClientTimerEndRequestPlug : IPipelinePlug<FieldOnPacketUserClientTimerEndRequest>
{
    public async Task Handle(IPipelineContext ctx, FieldOnPacketUserClientTimerEndRequest message)
    {
        if (message.User.Character.TemporaryStats.DashSpeedRecord?.Reason != Skill.Dual3HustleDash) return;
        await message.User.ModifyTemporaryStats(s => s.ResetDashSpeed());
    }
}
