using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public interface IMobStats
    {
        int this[MobStatType type] { get; }
        IDictionary<MobStatType, IMobStat> Stats { get; }
    }
}
