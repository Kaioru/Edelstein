using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects
{
    public abstract class AbstractControlledPacketHandler<T> : AbstractUserPacketHandler where T : IFieldControlledObj
    {
        public abstract FieldObjType Type { get; }

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var objID = packet.ReadInt();
            var obj = user.Field.GetPool(Type).GetObject(objID);

            if (obj is not T controlled) return;
            
            if (controlled == null) return;
            if (controlled.Controller != user) return;

            await Handle(stageUser, user, controlled, packet);
        }

        protected abstract Task Handle(
            GameStageUser stageUser,
            IFieldObjUser controller,
            T controlled,
            IPacketReader packet
        );
    }
}
