using Edelstein.Common.Gameplay.Accounts;

namespace Edelstein.Common.Database.Entities;

public record AccountEntity : Account
{
    public ICollection<AccountWorldEntity> AccountWorlds { get; set; }
}
