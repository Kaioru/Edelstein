using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts;

public record CheckDuplicatedID(
    ILoginStageUser User,
    string Name
) : ICheckDuplicatedID;
