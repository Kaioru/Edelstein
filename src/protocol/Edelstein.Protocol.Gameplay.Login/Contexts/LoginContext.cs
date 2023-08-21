namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContext(
    ILoginStageOptions Options,
    LoginContextPipelines Pipelines,
    LoginContextServices Services
);
