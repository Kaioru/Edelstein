using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;

namespace Edelstein.Common.Gameplay.Stages.Login.Messages;

public record EnableSPWRequest(
    ILoginStageUser User,
    int CharacterID,
    string MacAddress,
    string MacAddressWithHDDSerial,
    string SPW
) : IEnableSPWRequest;
