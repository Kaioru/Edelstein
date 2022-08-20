using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface IEnableSPWRequest : IStageUserContract<ILoginStageUser>
{
    int CharacterID { get; }
    string MacAddress { get; }
    string MacAddressWithHDDSerial { get; }
    string SPW { get; }
}
