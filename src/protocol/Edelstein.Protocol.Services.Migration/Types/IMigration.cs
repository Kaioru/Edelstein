using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Services.Migration.Types;

public interface IMigration : IIdentifiable<int>
{
    string FromServerID { get; }
    string ToServerID { get; }

    long Key { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }
}
