using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICheckSPWRequest : IStageUserContract<ILoginStageUser>
{
    string SPW { get; }
    int CharacterID { get; }
    string MacAddress { get; }
    string MacAddressWithHDDSerial { get; }
}
