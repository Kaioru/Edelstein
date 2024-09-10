using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Repositories;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IField : IRepositoryEntry<int>, IFieldObjectPool
{
    IFieldTemplate Template { get; }
    
    IFieldSplit? GetSplit(IPoint2D position);
    IFieldSplit?[] GetSplits(IRect2D bounds);
    IFieldSplit?[] GetEnclosingSplits(IPoint2D position);
    IFieldSplit?[] GetEnclosingSplits(IFieldSplit split);
    
    IFieldObjectPool? GetPool(FieldObjectType type);
}
