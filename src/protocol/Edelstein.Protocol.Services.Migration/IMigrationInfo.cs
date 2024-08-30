using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Protocol.Services.Migration;

public interface IMigrationInfo
{
    int AccountID { get; }
    int AccountWorldDataID { get; }
    int CharacterID { get; }

    string FromServerID { get; }
    string ToServerID { get; }
    
    MigrationInfoSnapshot<Account> AccountSnapshot { get; }
    MigrationInfoSnapshot<AccountWorldData>  AccountWorldDataSnapshot { get; }
    MigrationInfoSnapshot<Character> CharacterSnapshot { get; }
}
