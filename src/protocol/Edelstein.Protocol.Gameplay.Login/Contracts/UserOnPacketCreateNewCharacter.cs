namespace Edelstein.Protocol.Gameplay.Login.Contracts;

public record UserOnPacketCreateNewCharacter(
    ILoginStageUser User,
    string Name,
    int Race,
    short SubJob,
    int Face,
    int Hair,
    int HairColor,
    int Skin,
    int Coat,
    int Pants,
    int Shoes,
    int Weapon,
    byte Gender
);
