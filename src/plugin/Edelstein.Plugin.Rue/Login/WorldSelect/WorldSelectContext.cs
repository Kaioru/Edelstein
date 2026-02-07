using Edelstein.Plugin.Rue.ClientAnalysis;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Diagnostics;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.Login.WorldSelect;

public sealed record WorldSelectContext(
    ILogger? Logger,
    RueConfigLogin Config,
    LoginDiagnostics? Diagnostics,
    MemoryContext? MemoryContext,
    LoginMilestonesTracker? Tracker,
    MemoryWriter Writer,
    LoginStepMonitor? Monitor,
    LoginContext Context,
    ILoginStageUser User,
    int WorldId,
    int ChannelId);
