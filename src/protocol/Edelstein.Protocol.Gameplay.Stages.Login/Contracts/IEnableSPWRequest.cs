using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface IEnableSPWRequest : IStageUserMessage<ILoginStageUser>
{
    int CharacterID { get; }
    string MacAddress { get; }
    string MacAddressWithHDDSerial { get; }
    string SPW { get; }
}
