using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextRepositories(
    IAccountRepository Account,
    IAccountWorldRepository AccountWorld,
    ICharacterRepository Character
);
