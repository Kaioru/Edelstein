using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public interface IMobBurnedStat
    {
        IEnumerable<IMobBurnedStatInfo> BurnedInfo { get; }
    }
}
