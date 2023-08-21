namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketEnableSPWRequest(
    ILoginStageUser User,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial,
    string SPW
);
