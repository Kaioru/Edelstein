using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Services.Server;

public interface IServerInfoRepository : IQueriedRepository<string, ServerServiceServerInfo>;
