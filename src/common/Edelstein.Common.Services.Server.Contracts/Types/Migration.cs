using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Services.Migration.Types;

namespace Edelstein.Common.Services.Server.Contracts.Types;

public record Migration : IMigration
{
    public int AccountID { get; set; }
    public int CharacterID { get; set; }

    public string FromServerID { get; set; }
    public string ToServerID { get; set; }

    public long Key { get; set; }

    public IAccount Account { get; set; }
    public IAccountWorld AccountWorld { get; set; }
    public ICharacter Character { get; set; }
}
