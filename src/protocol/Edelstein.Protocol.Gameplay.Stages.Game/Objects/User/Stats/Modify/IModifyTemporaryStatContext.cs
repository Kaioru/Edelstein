using System;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Modify
{
    public interface IModifyTemporaryStatContext
    {
        void Set(
            SecondaryStatType type,
            int value,
            int reason,
            DateTime? dateExpire = null,
            bool isExpireAfterMigrate = false,
            bool isExpireAfterDisconnect = true
        );

        void ResetByType(SecondaryStatType type);
        void ResetByReason(int reason);
    }
}
