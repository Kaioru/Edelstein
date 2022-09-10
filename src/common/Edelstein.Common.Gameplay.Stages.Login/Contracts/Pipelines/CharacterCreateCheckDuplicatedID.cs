using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

public record CharacterCreateCheckDuplicatedID(
    ILoginStageUser User,
    string Name
) : ICharacterCreateCheckDuplicatedID;
