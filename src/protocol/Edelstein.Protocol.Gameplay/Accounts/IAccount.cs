using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Accounts;

public interface IAccount : IIdentifiable<int>
{
    string Username { get; }

    AccountGradeCode GradeCode { get; set; }
    AccountSubGradeCode SubGradeCode { get; set; }

    byte? Gender { get; set; }
}
