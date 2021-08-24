using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts.Social;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Social
{
    [ServiceContract]
    public interface IGuildService
    {
        [OperationContract] Task<GuildLoadByIDResponse> LoadByID(GuildLoadByIDRequest request);
        [OperationContract] Task<GuildLoadByCharacterResponse> LoadByCharacter(GuildLoadByCharacterRequest request);

        [OperationContract] IAsyncEnumerable<GuildUpdateContract> Subscribe(CallContext context = default);
    }
}
