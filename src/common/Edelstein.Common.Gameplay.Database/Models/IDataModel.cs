using Edelstein.Common.Gameplay.Database.Transformers;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Gameplay.Database.Models;

public interface IDataModel : IIdentifiable<int>, ITransformerContext
{
}
