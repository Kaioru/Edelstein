using System;
using System.Collections.Generic;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects.User
{
    public class QuestEffect : AbstractEffect
    {
        public override EffectType Type => EffectType.Quest;
        private ICollection<Tuple<int, int>> _entries;

        public QuestEffect(int templateID, int quantity)
        {
            _entries = new List<Tuple<int, int>> {Tuple.Create<int, int>(templateID, quantity)};
        }

        public QuestEffect(ICollection<Tuple<int, int>> entries)
        {
            _entries = entries;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<byte>((byte) _entries.Count);
            _entries.ForEach(e =>
            {
                packet.Encode<int>(e.Item1);
                packet.Encode<int>(e.Item2);
            });
        }
    }
}