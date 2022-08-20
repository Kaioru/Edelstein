using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContext(
    ILoginContextOptions Options,
    ILoginContextEvents Events,
    ILoginContextPipelines Pipelines,
    ILoginContextServices Services,
    ILoginContextManagers Managers,
    ILoginContextTemplates Templates
) : ILoginContext;
