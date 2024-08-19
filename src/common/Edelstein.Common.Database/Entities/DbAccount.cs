using System.Collections.Generic;
using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Common.Database.Entities;

public record DbAccount : Account
{
    public ICollection<DbAccountWorldData> AccountWorldData { get; set; }
    public DbMigrationInfo? Migration { get; set; }
}
