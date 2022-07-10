using System;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public interface IMobStat
    {
        MobStatType Type { get; }

        int Value { get; }
        int Reason { get; }

        DateTime? DateExpire { get; }
    }
}
