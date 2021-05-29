using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Protocol.Interop
{
    [ServiceContract]
    public interface IDispatchService
    {
        [OperationContract] Task<DispatchResponse> Dispatch(DispatchRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToAlliance(DispatchToAllianceRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToGuild(DispatchToGuildRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToParty(DispatchToPartyRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToCharacter(DispatchToCharacterRequest request);
    }
}
