using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts;

public record EnableSPWRequest(
    ILoginStageUser User,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial,
    string SPW
) : IEnableSPWRequest;
