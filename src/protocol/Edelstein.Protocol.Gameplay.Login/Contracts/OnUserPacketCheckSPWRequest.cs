namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record OnUserPacketCheckSPWRequest(
    ILoginStageUser User,
    string SPW,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial
);
