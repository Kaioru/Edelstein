using System;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Temporary
{
    public interface ITemporaryStat : ISecondaryStat
    {
        DateTime? DateExpire { get; }

        void WriteToPacketLocal(IPacketWriter writer);
        void WriteToPacketRemote(IPacketWriter writer);
    }
}
