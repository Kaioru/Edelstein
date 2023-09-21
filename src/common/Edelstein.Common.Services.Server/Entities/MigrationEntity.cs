using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Services.Server;

namespace Edelstein.Common.Services.Server.Entities;

public record MigrationEntity : IMigration
{

    public ServerEntity FromServer { get; set; }
    public ServerEntity ToServer { get; set; }

    public DateTime DateUpdated { get; set; }
    public DateTime DateExpire { get; set; }
    public int AccountID { get; set; }
    public int CharacterID { get; set; }

    public string FromServerID { get; set; }
    public string ToServerID { get; set; }

    public long Key { get; set; }

    public IAccount Account { get; set; }
    public IAccountWorld AccountWorld { get; set; }
    public ICharacter Character { get; set; }
}
