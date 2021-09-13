using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        IEnumerable<ITemporaryStat> TemporaryStats { get; }

        int GetStatOption(SecondaryStatType type);
        bool HasStatOption(SecondaryStatType type);
    }
}
