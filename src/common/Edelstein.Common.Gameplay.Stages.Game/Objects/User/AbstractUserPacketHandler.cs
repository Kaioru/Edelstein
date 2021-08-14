using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User
{
    public abstract class AbstractUserPacketHandler : AbstractPacketHandler<GameStage, GameStageUser>
    {
        public override async Task<bool> Check(GameStageUser user)
            => user.Field != null && user.FieldUser != null && await Check(user, user.FieldUser);

        public override Task Handle(GameStageUser user, IPacketReader packet)
            => Handle(user, user.FieldUser, packet);

        protected virtual Task<bool> Check(GameStageUser stageUser, IFieldObjUser user) => Task.FromResult(true);
        protected abstract Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet);
    }
}
