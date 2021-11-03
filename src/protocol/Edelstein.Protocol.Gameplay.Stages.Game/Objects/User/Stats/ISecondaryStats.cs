using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        int this[SecondaryStatType type] { get; }
        IDictionary<SecondaryStatType, ITemporaryStat> Stats { get; }
    }
}
