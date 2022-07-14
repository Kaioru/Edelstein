namespace Edelstein.Protocol.Gameplay.Stages.Login;

public enum LoginState
{
    CheckPassword = 0x0,
    SelectGender = 0x1,
    SelectWorld = 0x2,
    SelectCharacter = 0x3,
    Connecting = 0x4
}
