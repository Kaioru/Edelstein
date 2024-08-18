using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Common.Gameplay.Entities;

public record Account : IAccount
{
    public int ID { get; set; }

    public required string Username { get; set; }

    public string? PIN { get; set; }
    public string? SPW { get; set; }

    public AccountGradeCode GradeCode { get; set; }
    public AccountSubGradeCode SubGradeCode { get; set; }

    public byte? Gender { get; set; }
    
    public int NexonCash { get; set; }
    public int MaplePoint { get; set; }
    public int PrepaidNXCash { get; set; }
}
