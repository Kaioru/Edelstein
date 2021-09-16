using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        IEnumerable<ITemporaryStat> TemporaryStats { get; }

        bool HasStatOption(SecondaryStatType type);
        int GetStatOption(SecondaryStatType type);

        ITemporaryStat GetStat(SecondaryStatType type);
        ITwoStateGuidedBulletStat GetGuidedBulletStat() => GetStat(SecondaryStatType.GuidedBullet) as ITwoStateGuidedBulletStat;
        ITwoStatePartyBoosterStat GetPartyBoosterStat() => GetStat(SecondaryStatType.PartyBooster) as ITwoStatePartyBoosterStat;
    }
}
