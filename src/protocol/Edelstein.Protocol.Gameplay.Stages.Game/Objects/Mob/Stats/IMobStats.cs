using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats
{
    public interface IMobStats
    {
        bool HasStat(MobStatType type);

        int GetValue(MobStatType type);
        int GetReason(MobStatType type);

        IMobStat GetStat(MobStatType type);

        IDictionary<MobStatType, IMobStat> ToDictionary();
    }
}
