using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;

namespace Edelstein.Protocol.Services.Server;

public interface IMigration
{
    int AccountID { get; }
    int CharacterID { get; }

    string FromServerID { get; }
    string ToServerID { get; }

    long Key { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }
}
