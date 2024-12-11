using System.Collections.Generic;
using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Common.Database.Entities.Services.Session;
using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Common.Database.Entities;

public record DbAccount : Account
{
    public required ICollection<DbAccountWorldData> AccountWorldData { get; set; }
    public DbSessionInfo? Session { get; set; }
    public DbMigrationInfo? Migration { get; set; }
}
