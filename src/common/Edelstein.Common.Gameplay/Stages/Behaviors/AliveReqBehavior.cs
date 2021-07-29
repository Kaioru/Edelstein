using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages.Behaviors
{
    public class AliveReqBehavior<TStage, TUser> : ITickerBehavior
        where TStage : AbstractMigrateableStage<TStage, TUser>
        where TUser : AbstractMigrateableStageUser<TStage, TUser>
    {
        private readonly TStage _stage;

        public AliveReqBehavior(TStage stage)
            => _stage = stage;

        public Task OnTick(DateTime now)
            => Task.WhenAll(_stage.Users.Select(u => u.TrySendAliveReq()));
    }
}
