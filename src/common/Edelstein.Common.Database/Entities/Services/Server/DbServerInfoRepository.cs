using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities.Services.Server;

public class DbServerInfoRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : DbRepository<string, DbServerInfo, ServerServiceServerInfo>(factory, mapper, db => db.ServerInfo), 
    IServerInfoRepository;
