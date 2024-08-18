namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContext(
    LoginContextRepositories Repositories,
    LoginContextServices Services,
    LoginContextManagers Managers,
    LoginContextPipelines Pipelines
);
