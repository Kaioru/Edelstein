using AutoMapper;
using Edelstein.Common.Database;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Migration;

public partial class MigrationService(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper,
    IDateTimeProvider dateTimeProvider
) : IMigrationService;
