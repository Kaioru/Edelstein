using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Accounts;

public interface IAccount : IIdentifiable<int>
{
    string Username { get; }

    string? PIN { get; set; }
    string? SPW { get; set; }

    AccountGradeCode GradeCode { get; set; }
    AccountSubGradeCode SubGradeCode { get; set; }

    byte? Gender { get; set; }
}
