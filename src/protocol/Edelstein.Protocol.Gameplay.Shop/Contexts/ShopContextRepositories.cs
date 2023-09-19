using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Gameplay.Shop.Contexts;

public record ShopContextRepositories(
    IAccountRepository Account,
    IAccountWorldRepository AccountWorld,
    ICharacterRepository Character
);
