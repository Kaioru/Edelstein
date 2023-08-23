using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Services.Migration.Contracts;

public record Migration(
    int AccountID,
    int CharacterID,
    string FromServerID,
    string ToServerID,
    long Key,
    IAccount Account,
    IAccountWorld AccountWorld,
    ICharacter Character
) : IMigration;
