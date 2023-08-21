using Edelstein.Common.Gameplay.Models.Accounts;

namespace Edelstein.Common.Database.Entities;

public record AccountEntity : Account
{
    public ICollection<AccountWorldEntity> AccountWorlds { get; set; }
}
