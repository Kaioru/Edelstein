using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Repositories;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IField : IIdentifiable<int>, IFieldObjectPool
{
    IFieldTemplate Template { get; }

    IFieldSplit? GetSplit(IPoint2D position);
    IFieldSplit?[] GetEnclosingSplits(IPoint2D position);
    IFieldSplit?[] GetEnclosingSplits(IFieldSplit split);

    IFieldObjectPool? GetPool(FieldObjectType type);

    Task Enter(IFieldUser user);
    Task Leave(IFieldUser user);

    Task Enter(IFieldUser user, byte portal, Func<IPacket>? getEnterPacket = null);
    Task Enter(IFieldUser user, string portal, Func<IPacket>? getEnterPacket = null);

    Task Enter(IFieldObject obj, Func<IPacket>? getEnterPacket = null);
    Task Leave(IFieldObject obj, Func<IPacket>? getLeavePacket = null);

    T? GetObject<T>(int id) where T : IFieldObject;
    IEnumerable<T> GetObjects<T>() where T : IFieldObject;
}
