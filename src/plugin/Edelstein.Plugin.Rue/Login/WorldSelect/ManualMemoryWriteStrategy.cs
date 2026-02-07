using Edelstein.Plugin.Rue.ClientAnalysis;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Login.WorldSelect;

public sealed class ManualMemoryWriteStrategy : IWorldSelectStrategy
{
    public async Task Execute(WorldSelectContext context)
    {
        var t0 = Environment.TickCount64;
        var writer = context.Writer;

        context.Logger?.LogInformation("[Rue-AutoLogin] Writing CWvsContext world={WorldId}, ch={ChannelId} (manual-write)", context.WorldId, context.ChannelId);

        var preLoginStep = writer.ReadLoginStep();
        var preStepChanging = writer.ReadStepChanging();
        var preRequestSent = writer.ReadRequestSent();
        var preWorldId = writer.ReadWorldId();
        var preChannelId = writer.ReadChannelId();
        var preSelectedChannel = writer.ReadChannelSelectSelected();
        var preWorldItemPtr = writer.ReadChannelSelectWorldItemPtr();
        var preChannelNamePtr = writer.ReadChannelNameArrayPtr();
        var preAdultChannelPtr = writer.ReadAdultChannelArrayPtr();

        context.Logger?.LogDebug("[Rue-AutoLogin] Pre-write: Step={Step}, StepChg={StepChg}, ReqSent={ReqSent}",
            preLoginStep, preStepChanging, preRequestSent);

        context.Diagnostics?.RecordMilestone("MemoryWriteWorldChannel");
        context.Diagnostics?.LogMemoryWrite("CWvsContext->m_nWorldID", preWorldId, context.WorldId);
        context.Diagnostics?.LogMemoryWrite("CWvsContext->m_nChannelID", preChannelId, context.ChannelId);

        if (!writer.SetWorldAndChannel(context.WorldId, context.ChannelId))
        {
            context.Logger?.LogError("[Rue-AutoLogin] Failed to write world/channel to CWvsContext");
            context.Diagnostics?.LogError("AutoLogin", "Failed to write world/channel to CWvsContext");
            return;
        }

        context.Diagnostics?.LogMemoryWrite("CQuestMan->m_nWorldID", null, context.WorldId);
        if (!writer.SetQuestManWorldId(context.WorldId))
            context.Logger?.LogWarning("[Rue-AutoLogin] Failed to mirror WorldID to CQuestMan — ChangeStep may crash");
        else
            context.Logger?.LogInformation("[Rue-AutoLogin] Mirrored CQuestMan.m_nWorldID = {WorldId}", context.WorldId);

        if (context.Monitor != null)
        {
            var stableOk = await context.Monitor.WaitWithTimeout(
                ct => context.Monitor.WaitForStepTransitionComplete(ct),
                "StepTransitionComplete before m_bRequestSent");

            if (!stableOk)
                context.Logger?.LogWarning("[Rue-AutoLogin] Step transition timeout - proceeding anyway");
        }
        else
        {
            var delay = context.Config.AutoSelectDelayMs;
            if (delay > 0)
            {
                context.Logger?.LogInformation("[Rue-AutoLogin] Waiting {Delay}ms for step transition (delay-based)", delay);
                await Task.Delay(delay);
            }
        }

        var elapsed = Environment.TickCount64 - t0;
        context.Logger?.LogInformation("[Rue-AutoLogin] Step transition complete (+{Elapsed})", LoginMilestonesTracker.FormatElapsed(elapsed));

        context.Diagnostics?.RecordMilestone("FixupChannelSelectAndContext");

        if (context.Config.ClientMemory?.FixupChannelSelectState ?? true)
        {
            if (writer.EnsureChannelSelectState(context.WorldId, context.ChannelId))
            {
                var selected = writer.ReadChannelSelectSelected();
                var worldItemPtr = writer.ReadChannelSelectWorldItemPtr();
                context.Logger?.LogInformation("[Rue-AutoLogin] Fixup: CUIChannelSelect selected={Selected}, worldItemPtr=0x{Ptr:X8}",
                    selected,
                    worldItemPtr ?? 0);

                context.Diagnostics?.LogMemoryWrite("CUIChannelSelect->m_nSelect", preSelectedChannel, selected);
                context.Diagnostics?.LogMemoryWrite("CUIChannelSelect->m_pWorldItem", preWorldItemPtr, worldItemPtr);
            }
            else
            {
                context.Logger?.LogWarning("[Rue-AutoLogin] Fixup failed to set CUIChannelSelect world item / selection");
            }
        }

        if (writer.EnsureContextChannelArraysFromWorldItem(context.WorldId))
        {
            var channelNamePtr = writer.ReadChannelNameArrayPtr();
            var adultChannelPtr = writer.ReadAdultChannelArrayPtr();
            context.Logger?.LogInformation("[Rue-AutoLogin] Fixup: CWvsContext channel arrays set (names=0x{Names:X8}, adult=0x{Adult:X8})",
                channelNamePtr ?? 0,
                adultChannelPtr ?? 0);

            context.Diagnostics?.LogMemoryWrite("CWvsContext->m_aChannelName.a", preChannelNamePtr, channelNamePtr);
            context.Diagnostics?.LogMemoryWrite("CWvsContext->m_aAdultChannel.a", preAdultChannelPtr, adultChannelPtr);
        }
        else
        {
            context.Logger?.LogWarning("[Rue-AutoLogin] Fixup failed to set CWvsContext channel arrays");
        }

        if (writer.FindCLogin())
        {
            context.Diagnostics?.RecordMilestone("SetRequestSent");
            context.Diagnostics?.LogMemoryWrite("CLogin->m_bRequestSent", preRequestSent, 1);

            if (!writer.SetRequestSent(true))
                context.Logger?.LogWarning("[Rue-AutoLogin] Failed to set CLogin.m_bRequestSent");
            else
                context.Logger?.LogInformation("[Rue-AutoLogin] Set CLogin.m_bRequestSent = 1");
        }
        else
        {
            context.Logger?.LogWarning("[Rue-AutoLogin] CLogin not found - cannot set m_bRequestSent");
        }

        context.Logger?.LogInformation("[Rue-AutoLogin] Sending SelectWorldResult");

        context.Diagnostics?.RecordMilestone("SelectWorldResult");
        context.Diagnostics?.LogPacketSent("SelectWorldResult", 0x0B,
            new Dictionary<string, object?>
            {
                ["worldId"] = context.WorldId,
                ["channelId"] = context.ChannelId
            });

        await context.Context.Pipelines.UserOnPacketSelectWorld.Process(new UserOnPacketSelectWorld(
            context.User,
            context.WorldId,
            context.ChannelId
        ));

        context.Diagnostics?.RecordMilestone("SelectCharacterState");
    }

}
