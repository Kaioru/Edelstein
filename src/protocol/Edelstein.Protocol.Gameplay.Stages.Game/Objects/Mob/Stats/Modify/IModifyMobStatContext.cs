using System;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats.Modify
{
    public interface IModifyMobStatContext
    {
        IMobStats SetHistory { get; }
        IMobStats ResetHistory { get; }

        void Set(IMobStat stat);
        void Set(
            MobStatType type,
            int value,
            int reason,
            DateTime? dateExpire = null
        );

        void Reset(IMobStat stat);
        void ResetByType(MobStatType type);
        void ResetByReason(int reason);
    }
}
