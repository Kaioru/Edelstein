using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Protocol.Services.Social
{
    [ServiceContract]
    public interface IInviteService
    {
        [OperationContract] Task<InviteRegisterResponse> Register(InviteRegisterRequest request);
        [OperationContract] Task<InviteDeregisterResponse> Deregister(InviteDeregisterRequest request);
        [OperationContract] Task<InviteDeregisterAllResponse> DeregisterAll(InviteDeregisterAllRequest request);
        [OperationContract] Task<InviteClaimResponse> Claim(InviteClaimRequest request);
    }
}
