using Edelstein.Protocol.Gameplay.Entities;

namespace Edelstein.Common.Database.Entities;

public class DbAccountWorldData : AccountWorldData
{
    public required DbAccount Account { get; set; }
}
