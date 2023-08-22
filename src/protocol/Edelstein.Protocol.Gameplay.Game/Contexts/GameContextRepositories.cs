using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextRepositories(
    IAccountRepository Account,
    IAccountWorldRepository AccountWorld,
    ICharacterRepository Character
);
