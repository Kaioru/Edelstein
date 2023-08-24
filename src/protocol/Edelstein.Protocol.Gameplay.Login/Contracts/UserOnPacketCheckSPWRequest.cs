namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketCheckSPWRequest(
    ILoginStageUser User,
    string SPW,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial
);
