using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Handling
{
    public abstract class AbstractPacketHandler<TStage, TUser> : IPacketHandler<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        public abstract short Operation { get; }

        public virtual Task<bool> Check(TUser user) => Task.FromResult(true);
        public abstract Task Handle(TUser user, IPacketReader packet);
    }
}
