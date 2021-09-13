using System;
using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary
{
    public interface ITemporaryStat : IPacketWritable
    {
        SecondaryStatType Type { get; }

        int Value { get; }
        int Reason { get; }

        DateTime? DateExpire { get; }
    }
}
