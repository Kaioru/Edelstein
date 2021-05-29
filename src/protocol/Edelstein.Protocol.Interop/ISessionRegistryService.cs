using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Protocol.Interop
{
    [ServiceContract]
    public interface ISessionRegistryService
    {
        [OperationContract] Task<UpdateSessionResult> UpdateSession(UpdateSessionRequest request);
        [OperationContract] Task<DescribeSessionResult> DescribeSessionByAccount(DescribeSessionByAccountRequest request);
        [OperationContract] Task<DescribeSessionResult> DescribeSessionByCharacter(DescribeSessionByCharacterRequest request);
    }
}
