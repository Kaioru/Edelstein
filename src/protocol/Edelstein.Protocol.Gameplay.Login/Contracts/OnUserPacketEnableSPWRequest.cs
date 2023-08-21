namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record OnUserPacketEnableSPWRequest(
    ILoginStageUser User,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial,
    string SPW
);
