namespace Edelstein.Protocol.Gameplay.Stages.Login;

public enum LoginState
{
    CheckPassword = 0x0,
    CheckToken = 0x1,
    SelectGender = 0x2,
    SelectWorld = 0x3,
    SelectCharacter = 0x4,
    Completed = 0x5
}
