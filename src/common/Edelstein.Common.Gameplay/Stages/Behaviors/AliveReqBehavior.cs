using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages.Behaviors
{
    public class AliveReqBehavior<TStage, TUser, TConfig> : ITickerBehavior
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : IServerStageInfo
    {
        private readonly TStage _stage;

        public AliveReqBehavior(TStage stage)
            => _stage = stage;

        public Task OnTick(DateTime now)
            => Task.WhenAll(_stage.GetUsers().Select(u => u.TrySendAliveReq()));
    }
}
