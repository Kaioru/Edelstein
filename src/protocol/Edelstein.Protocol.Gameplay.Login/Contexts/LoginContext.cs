using Duey.Abstractions;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContext(
    ITicker Ticker,
    IDataNamespace DataNamespace,
    LoginContextRepositories Repositories,
    LoginContextServices Services,
    LoginContextManagers Managers,
    LoginContextTemplates Templates,
    LoginContextPipelines Pipelines
);
