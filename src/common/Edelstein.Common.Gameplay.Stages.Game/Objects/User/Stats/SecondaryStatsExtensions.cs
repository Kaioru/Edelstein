using Edelstein.Common.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public static class SecondaryStatsExtensions
    {
        public static void WriteToLocal(this ISecondaryStats stats, IPacketWriter writer)
        {
            var flag = new Flags(128);

            writer.Write(flag);

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState
        }

        public static void WriteToRemote(this ISecondaryStats stats, IPacketWriter writer)
        {
            var flag = new Flags(128);

            writer.Write(flag);

            writer.WriteByte(0); // nDefenseAtt
            writer.WriteByte(0); // nDefenseState
        }
    }
}
