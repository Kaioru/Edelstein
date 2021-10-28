using System;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats.Temporary
{
    public record TemporaryStat : ITemporaryStat
    {
        public SecondaryStatType Type { get; }

        public int Value { get; }
        public int Reason { get; }

        public DateTime? DateExpire { get; }
    }
}
