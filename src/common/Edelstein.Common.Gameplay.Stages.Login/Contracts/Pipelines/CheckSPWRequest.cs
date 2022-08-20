using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

public record CheckSPWRequest(
    ILoginStageUser User,
    string SPW,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial
) : ICheckSPWRequest;
