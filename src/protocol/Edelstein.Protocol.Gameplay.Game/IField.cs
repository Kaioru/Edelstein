using Edelstein.Protocol.Gameplay.Game.Generators;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IField : IIdentifiable<int>, IFieldObjectPool
{
    IFieldManager Manager { get; }
    IFieldTemplate Template { get; }
    
    IFieldGeneratorRegistry Generators { get; }

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
}
