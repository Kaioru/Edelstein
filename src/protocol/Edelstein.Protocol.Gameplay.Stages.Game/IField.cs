using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Buffers.Bytes;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IField : IFieldObjectPool, IStage<IFieldUser>, IIdentifiable<int>
{
    IFieldTemplate Template { get; }

    IFieldObjectPool? GetObjectPool(FieldObjectType type);

    Task Enter(IFieldUser user, byte portal, Func<IPacket>? getEnterPacket = null);
    Task Enter(IFieldUser user, string portal, Func<IPacket>? getEnterPacket = null);

    Task Enter(IFieldObject obj, Func<IPacket>? getEnterPacket = null);
    Task Leave(IFieldObject obj, Func<IPacket>? getLeavePacket = null);
}
