using System.Collections.Generic;
using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Common.Database.Entities;

public record DbAccountWorldData : AccountWorldData
{
    public DbAccount Account { get; set; }
    public ICollection<DbCharacter> Characters { get; set; }
    
    public DbMigrationInfo? Migration { get; set; }
}
