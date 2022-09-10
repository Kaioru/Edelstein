using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

public record CharacterCreate(
    ILoginStageUser User,
    string Name,
    int Race,
    short SubJob,
    byte Gender,
    byte Skin,
    int[] Look
) : ICharacterCreate;
