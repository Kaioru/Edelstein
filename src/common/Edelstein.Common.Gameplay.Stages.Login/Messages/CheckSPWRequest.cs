using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;

namespace Edelstein.Common.Gameplay.Stages.Login.Messages;

public record CheckSPWRequest(
    ILoginStageUser User,
    string SPW,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial
) : ICheckSPWRequest;
