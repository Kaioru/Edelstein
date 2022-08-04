using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects;

public abstract class AbstractFieldObjectPool : IFieldObjectPool
{
    public abstract IReadOnlyCollection<IFieldObject> Objects { get; }

    public abstract Task Enter(IFieldObject obj);
    public abstract Task Leave(IFieldObject obj);

    public abstract IFieldObject? GetObject(int id);
    public abstract IEnumerable<IFieldObject> GetObjects();

    public virtual Task Dispatch(IPacket packet) =>
        Task.WhenAll(Objects
            .OfType<IAdapter>()
            .Select(a => a.Dispatch(packet)));

    public virtual Task Dispatch(IPacket packet, IFieldObject obj) =>
        Task.WhenAll(Objects
            .OfType<IAdapter>()
            .Where(a => a != obj)
            .Select(a => a.Dispatch(packet)));

    public IFieldUser? CreateUser(IGameStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
            return null;
        return new FieldUser(user, user.Account!, user.AccountWorld!, user.Character!);
    }

    public IFieldNPC CreateNPC(
        INPCTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        int rx0 = 0,
        int rx1 = 0,
        bool isFacingLeft = true,
        bool isEnabled = true
    ) => new FieldNPC(template, position, foothold, rx0, rx1, isFacingLeft, isEnabled);
}
