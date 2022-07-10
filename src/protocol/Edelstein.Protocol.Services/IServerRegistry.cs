using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Protocol.Services
{
    [ServiceContract]
    public interface IServerRegistry
    {
        [OperationContract] Task<RegisterServerResponse> Register(RegisterServerRequest request);
        [OperationContract] Task<DeregisterServerResponse> Deregister(DeregisterServerRequest request);
        [OperationContract] Task<UpdateServerResponse> Update(UpdateServerRequest request);
        [OperationContract] Task<DescribeServerByIDResponse> DescribeByID(DescribeServerByIDRequest request);
        [OperationContract] Task<DescribeServerByMetadataResponse> DescribeByMetadata(DescribeServerByMetadataRequest request);
    }
}
