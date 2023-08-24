namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContext(
    ILoginStageOptions Options,
    LoginContextManagers Managers,
    LoginContextServices Services,
    LoginContextRepositories Repositories,
    LoginContextTemplates Templates,
    LoginContextPipelines Pipelines
);
