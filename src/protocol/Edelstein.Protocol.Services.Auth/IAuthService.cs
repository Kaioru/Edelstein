using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Auth.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Auth;

[ServiceContract]
public interface IAuthService
{
    [OperationContract]
    Task<AuthServiceResponse> Login(AuthServiceRequest request, CallContext context = default);
    
    [OperationContract]
    Task<AuthServiceResponse> Register(AuthServiceRequest request, CallContext context = default);
}
