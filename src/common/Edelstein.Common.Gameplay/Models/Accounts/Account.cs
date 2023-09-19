using Edelstein.Protocol.Gameplay.Models.Accounts;

namespace Edelstein.Common.Gameplay.Models.Accounts;

public record Account : IAccount
{
    public int ID { get; set; }

    public string Username { get; set; }

    public string? PIN { get; set; }
    public string? SPW { get; set; }

    public AccountGradeCode GradeCode { get; set; }
    public AccountSubGradeCode SubGradeCode { get; set; }

    public byte? Gender { get; set; }
    
    public int NexonCash { get; set; }
    public int MaplePoint { get; set; }
    public int PrepaidNXCash { get; set; }
}
