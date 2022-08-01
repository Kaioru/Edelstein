using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Gameplay.Database.Models;

public interface IDataModel : IIdentifiable<int>
{
    public byte[] Bytes { get; set; }
}
