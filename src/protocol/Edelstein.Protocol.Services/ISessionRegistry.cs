using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Protocol.Services
{
    [ServiceContract]
    public interface ISessionRegistry
    {
        [OperationContract] Task<StartSessionResponse> Start(StartSessionRequest request);
        [OperationContract] Task<UpdateSessionResponse> Update(UpdateSessionRequest request);
        [OperationContract] Task<DescribeSessionByAccountResponse> DescribeByAccount(DescribeSessionByAccountRequest request);
        [OperationContract] Task<DescribeSessionByCharacterResponse> DescribeByCharacter(DescribeSessionByCharacterRequest request);
    }
}
