using System;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public record MobStat : IMobStat
    {
        public MobStatType Type { get; init; }

        public int Value { get; init; }
        public int Reason { get; init; }

        public DateTime? DateExpire { get; init; }
    }
}
