using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface IEnableSPWRequest : IStageUserMessage<ILoginStageUser>
{
    int CharacterID { get; }
    string MacAddress { get; }
    string MacAddressWithHDDSerial { get; }
    string SPW { get; }
}
