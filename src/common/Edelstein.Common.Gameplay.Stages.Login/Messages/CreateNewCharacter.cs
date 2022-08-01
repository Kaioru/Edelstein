using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;

namespace Edelstein.Common.Gameplay.Stages.Login.Messages;

public record CreateNewCharacter(
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
) : ICreateNewCharacter;
