namespace Edelstein.Protocol.Gameplay.Accounts;

[Flags]
public enum AccountGradeCode : byte
{
    AdminLevel1 = 0x1,
    AdminLevel2 = 0x2,
    AdminLevel3 = 0x4,
    AdminLevel4 = 0x8,
    AdminLevel5 = 0x10,
    AdminLevel10 = 0x20
}
