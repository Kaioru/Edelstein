using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Common.Database.Entities;

public record DbAccount : Account
{
    public required ICollection<DbAccountWorldData> AccountWorldData { get; set; }
}
