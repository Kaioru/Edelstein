using System.Threading.Tasks;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Entities;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public partial class ServerService
{
    public Task<ServerServiceRegisterResponse> RegisterLogin(ServerServiceRegisterRequest<ServerServiceServerInfoLogin> request, CallContext context = default)
        => Register(mapper.Map<DbServerInfoLogin>(request.Info));
}
