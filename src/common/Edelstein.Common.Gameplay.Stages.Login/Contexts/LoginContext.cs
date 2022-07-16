using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContext(
    ILoginContextOptions Options,
    ILoginContextPipelines Pipelines
) : ILoginContext;
