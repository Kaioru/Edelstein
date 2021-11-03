using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        bool HasStat(SecondaryStatType type);

        int GetValue(SecondaryStatType type);
        int GetReason(SecondaryStatType type);

        ITemporaryStat GetStat(SecondaryStatType type);

        IDictionary<SecondaryStatType, ITemporaryStat> ToDictionary();
    }
}
