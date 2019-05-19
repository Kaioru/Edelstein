using System;
using System.Collections;
using System.Collections.Generic;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.Objects.User.Stats
{
    public static class TemporaryStatExtensions
    {
        public static void EncodeMask(this IDictionary<TemporaryStatType, TemporaryStat> stats, IPacket packet)
        {
            var bits = new BitArray(128);
            var array = new int[4];

            stats.Keys.ForEach(t => bits[(int) t] = true);
            bits.CopyTo(array, 0);
            for (var i = 4; i > 0; i--) packet.Encode<int>(array[i - 1]);
        }

        public static void EncodeLocal(this IDictionary<TemporaryStatType, TemporaryStat> stats, IPacket packet)
        {
            stats.EncodeMask(packet);

            TemporaryStatOrder.EncodingOrderLocal.ForEach(t =>
            {
                if (!stats.ContainsKey(t)) return;

                packet.Encode<short>((short) stats[t].Option);
                packet.Encode<int>(stats[t].TemplateID);
                packet.Encode<int>(stats[t].DateExpire.HasValue
                    ? (int) (stats[t].DateExpire.Value - DateTime.Now).TotalMilliseconds
                    : int.MaxValue);
            });

            packet.Encode<byte>(0); // nDefenseAtt
            packet.Encode<byte>(0); // nDefenseState

            if (stats.ContainsKey(TemporaryStatType.SwallowAttackDamage) &&
                stats.ContainsKey(TemporaryStatType.SwallowDefence) &&
                stats.ContainsKey(TemporaryStatType.SwallowCritical) &&
                stats.ContainsKey(TemporaryStatType.SwallowMaxMP) &&
                stats.ContainsKey(TemporaryStatType.SwallowEvasion))
                packet.Encode<byte>(0);
            if (stats.ContainsKey(TemporaryStatType.Dice))
                for (var i = 0; i < 22; i++)
                    packet.Encode<int>(0);
            if (stats.ContainsKey(TemporaryStatType.BlessingArmor))
                packet.Encode<int>(0);

            EncodeTwoState(stats, packet);
        }

        public static void EncodeRemote(this IDictionary<TemporaryStatType, TemporaryStat> stats, IPacket packet)
        {
            stats.EncodeMask(packet);

            TemporaryStatOrder.EncodingOrderRemote.ForEach(kv =>
            {
                if (!stats.ContainsKey(kv.Key)) return;
                kv.Value.Invoke(stats[kv.Key], packet);
            });

            packet.Encode<byte>(0); // nDefenseAtt
            packet.Encode<byte>(0); // nDefenseState

            EncodeTwoState(stats, packet);
        }

        public static void EncodeTwoState(this IDictionary<TemporaryStatType, TemporaryStat> stats, IPacket packet)
        {
            var now = DateTime.Now;

            TemporaryStatOrder.EncodingTwoStateOrderRemote.ForEach(type =>
            {
                if (!stats.ContainsKey(type)) return;
                var stat = stats[type];

                packet.Encode<int>(stat.Option);
                packet.Encode<int>(stat.TemplateID);

                if (stat.DateExpire.HasValue)
                {
                    // TODO: proper last updated
                    packet.Encode<bool>(now > stat.DateExpire.Value);
                    packet.Encode<int>((int) (now - stat.DateExpire.Value).TotalSeconds);
                }
                else
                {
                    packet.Encode<bool>(true);
                    packet.Encode<int>(int.MaxValue);
                }
                
                // TODO: Dynamic term encode ExpireTerm short

                // TODO: PartyBooster
                // TODO: GuidedBullet
            });
        }
    }
}