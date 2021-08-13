using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User
{
    public abstract class AbstractUserPacketHandler : AbstractPacketHandler<GameStage, GameStageUser>
    {
        public override short Operation => throw new System.NotImplementedException();

        public override async Task<bool> Check(GameStageUser user)
            => user.Field != null && user.FieldUser != null && await Check(user.FieldUser);

        public override Task Handle(GameStageUser user, IPacketReader packet)
            => Handle(user.FieldUser, packet);

        protected virtual Task<bool> Check(IFieldObjUser user) => Task.FromResult(true);
        protected abstract Task Handle(IFieldObjUser user, IPacketReader packet);
    }
}
