namespace Edelstein.Protocol.Gameplay.Models.Accounts;

[Flags]
public enum AccountSubGradeCode : short
{
    PrimaryTrace = 0x1,
    SecondaryTrace = 0x2,
    AdminClient = 0x4,
    MobMoveObserve = 0x8,
    ManagerAccount = 0x10,
    OutSourceSuperGM = 0x20,
    OutSourceGM = 0x40,
    UserGM = 0x80,
    TesterAccount = 0x100
}
