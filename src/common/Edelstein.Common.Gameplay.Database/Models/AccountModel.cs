using Edelstein.Protocol.Gameplay.Accounts;

namespace Edelstein.Common.Gameplay.Database.Models;

public record AccountModel : IAccount
{
    public int ID { get; set; }

    public string Username { get; set; }

    public AccountGradeCode GradeCode { get; set; }
    public AccountSubGradeCode SubGradeCode { get; set; }

    public byte? Gender { get; set; }
}
