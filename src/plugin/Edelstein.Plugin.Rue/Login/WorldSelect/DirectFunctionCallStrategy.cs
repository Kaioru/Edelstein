using Edelstein.Plugin.Rue.ClientAnalysis;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Login.WorldSelect;

public sealed class DirectFunctionCallStrategy : IWorldSelectStrategy
{
    public async Task Execute(WorldSelectContext context)
    {
        var t0 = Environment.TickCount64;
        var writer = context.Writer;

        context.Logger?.LogInformation("[Rue-AutoLogin] DFC: calling SendLoginPacket({WorldId}, {ChannelId})", context.WorldId, context.ChannelId);

        if (!writer.FindCLogin())
        {
            context.Logger?.LogError("[Rue-AutoLogin] DFC: cannot find CLogin - ensure CUIChannelSelect exists");
            context.Diagnostics?.LogError("AutoLogin-DFC", "CLogin not found for direct function call");
            return;
        }

        context.Logger?.LogDebug("[Rue-AutoLogin] DFC pre-call: World={World}, Ch={Ch}, Step={Step}, ReqSent={ReqSent}",
            writer.ReadWorldId(), writer.ReadChannelId(),
            writer.ReadLoginStep(), writer.ReadRequestSent());

        context.Diagnostics?.RecordMilestone("DirectFunctionCall_SendLoginPacket");
        context.Diagnostics?.LogMemoryWrite("RemoteCall", "CLogin::SendLoginPacket",
            $"worldId={context.WorldId}, channelId={context.ChannelId}");

        if (!writer.CallSendLoginPacket(context.WorldId, context.ChannelId))
        {
            context.Logger?.LogError("[Rue-AutoLogin] DFC: SendLoginPacket FAILED");
            context.Diagnostics?.LogError("AutoLogin-DFC", "CallSendLoginPacket failed");
            return;
        }

        var elapsed = Environment.TickCount64 - t0;
        context.Logger?.LogInformation("[Rue-AutoLogin] DFC: SendLoginPacket -> success (+{Elapsed})", LoginMilestonesTracker.FormatElapsed(elapsed));

        if (context.Monitor != null)
        {
            var stepOk = await context.Monitor.WaitWithTimeout(
                ct => context.Monitor.WaitForLoginStep(LoginStep.SelectCharacter, ct),
                "SelectCharacter");

            if (!stepOk)
                context.Logger?.LogWarning("[Rue-AutoLogin] DFC: SelectCharacter timeout - proceeding");
        }
        else
        {
            await Task.Delay(100);
        }

        context.Diagnostics?.RecordMilestone("DirectFunctionCall_Complete");

        context.Logger?.LogDebug("[Rue-AutoLogin] DFC post-call: Step={Step}, ReqSent={ReqSent}, CharSel={CharSel}",
            writer.ReadLoginStep(), writer.ReadRequestSent(), writer.ReadCharSelected());

        context.Logger?.LogInformation("[Rue-AutoLogin] DFC: complete ({TotalMs}ms)", Environment.TickCount64 - t0);
    }
}
