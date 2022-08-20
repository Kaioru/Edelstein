using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

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
