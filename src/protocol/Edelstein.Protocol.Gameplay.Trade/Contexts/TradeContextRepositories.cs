using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContextRepositories(
    IAccountRepository Account,
    IAccountWorldRepository AccountWorld,
    ICharacterRepository Character
);
