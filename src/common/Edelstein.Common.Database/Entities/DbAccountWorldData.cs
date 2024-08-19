using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Common.Database.Entities;

public class DbAccountWorldData : AccountWorldData
{
    public DbAccount Account { get; set; }
    public DbMigrationInfo? Migration { get; set; }
}
