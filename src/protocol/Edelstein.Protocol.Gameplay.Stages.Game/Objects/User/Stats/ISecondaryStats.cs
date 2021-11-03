using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats
{
    public interface ISecondaryStats
    {
        int this[SecondaryStatType type] { get; }
        IDictionary<SecondaryStatType, ITemporaryStat> Stats { get; }
    }
}
