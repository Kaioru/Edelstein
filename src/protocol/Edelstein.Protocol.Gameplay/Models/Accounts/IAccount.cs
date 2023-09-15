using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Models.Accounts;

public interface IAccount : IIdentifiable<int>
{
    string Username { get; }

    string? PIN { get; set; }
    string? SPW { get; set; }

    AccountGradeCode GradeCode { get; set; }
    AccountSubGradeCode SubGradeCode { get; set; }

    byte? Gender { get; set; }
    
    int NexonCash { get; set; }
    int MaplePoint { get; set; }
    int PrepaidNXCash { get; set; }
}
