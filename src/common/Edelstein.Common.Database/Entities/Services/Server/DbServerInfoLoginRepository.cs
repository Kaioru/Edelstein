using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities.Services.Server;

public class DbServerInfoLoginRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : DbRepository<string, DbServerInfoLogin, ServerServiceServerInfoLogin>(factory, mapper, db => db.ServerInfoLogin), 
    IServerInfoLoginRepository;
