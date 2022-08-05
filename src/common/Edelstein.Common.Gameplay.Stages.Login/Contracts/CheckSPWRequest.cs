using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts;

public record CheckSPWRequest(
    ILoginStageUser User,
    string SPW,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial
) : ICheckSPWRequest;
