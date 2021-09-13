using System;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary
{
    public interface ITwoStatePartyBoosterStat : ITwoStateTemporaryStat
    {
        TimeSpan? TimeCurrent { get; }
    }
}
