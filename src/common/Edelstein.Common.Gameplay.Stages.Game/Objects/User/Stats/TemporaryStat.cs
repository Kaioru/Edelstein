using System;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public record TemporaryStat : ITemporaryStat
    {
        public SecondaryStatType Type { get; init; }

        public int Value { get; init; }
        public int Reason { get; init; }

        public DateTime? DateExpire { get; init; }
    }
}
