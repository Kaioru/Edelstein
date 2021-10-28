using System;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary
{
    public interface ITemporaryStat
    {
        SecondaryStatType Type { get; }

        int Value { get; }
        int Reason { get; }

        DateTime? DateExpire { get; }
    }
}
