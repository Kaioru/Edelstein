using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects
{
    public abstract class AbstractControlledPacketHandler<T> : AbstractUserPacketHandler where T : IFieldControlledObj
    {
        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var objID = packet.ReadInt();
            var obj = user.Field.GetObject<T>(objID);

            if (obj == null) return;
            if (obj.Controller != user) return;

            await Handle(stageUser, user, obj, packet);
        }

        protected abstract Task Handle(
            GameStageUser stageUser,
            IFieldObjUser controller,
            T controlled,
            IPacketReader packet
        );
    }
}
