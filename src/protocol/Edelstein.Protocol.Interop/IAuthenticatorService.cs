using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Protocol.Interop
{
    [ServiceContract]
    public interface IAuthenticatorService
    {
        [OperationContract] Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
    }
}
