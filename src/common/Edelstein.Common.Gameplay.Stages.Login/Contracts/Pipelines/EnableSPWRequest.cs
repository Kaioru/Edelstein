using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

public record EnableSPWRequest(
    ILoginStageUser User,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial,
    string SPW
) : IEnableSPWRequest;
