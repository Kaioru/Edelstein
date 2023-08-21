namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketDeleteCharacter(
    ILoginStageUser User,
    string SPW,
    int CharacterID
);
