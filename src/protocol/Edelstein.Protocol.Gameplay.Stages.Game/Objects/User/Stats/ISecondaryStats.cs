using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        IEnumerable<ITemporaryStat> TemporaryStats { get; }

        bool HasStatOption(SecondaryStatType type);
        int GetStatOption(SecondaryStatType type);

        ITwoStateGuidedBulletStat GetGuidedBulletStat();
        ITwoStatePartyBoosterStat GetPartyBoosterStat();
    }
}
