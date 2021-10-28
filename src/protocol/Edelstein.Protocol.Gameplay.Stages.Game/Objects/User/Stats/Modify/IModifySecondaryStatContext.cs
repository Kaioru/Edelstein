using System;
using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Modify
{
    public interface IModifySecondaryStatContext
    {
        ISecondaryStats SetHistory { get; }
        ISecondaryStats ResetHistory { get; }

        void Set(ITemporaryStat stat);
        void Set(
            SecondaryStatType type,
            int value,
            int reason,
            DateTime? dateExpire = null
        );

        void Reset(ITemporaryStat stat);
        void ResetByType(SecondaryStatType type);
        void ResetByReason(int reason);
    }
}
