using AutoMapper;
using Edelstein.Common.Database;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Session;

public partial class SessionService(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper,
    IDateTimeProvider dateTimeProvider
) : ISessionService;
