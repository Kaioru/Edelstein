namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContext(
    LoginContextManagers Managers,
    LoginContextRepositories Repositories,
    LoginContextPipelines Pipelines
);
