using Edelstein.Common.Database;
using Edelstein.Protocol.Services.Session;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Session;

public partial class SessionService(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : ISessionService;
