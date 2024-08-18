using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextRepositories(
    IAccountRepository Account,
    IAccountWorldDataRepository AccountWorldData
);
