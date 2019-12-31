using System;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects
{
    public abstract class AbstractFieldObj : IFieldObj
    {
        public abstract FieldObjType Type { get; }

        public int ID { get; set; }

        public IField Field { get; set; }

        public IFieldSplit SplitCenter => SplitEnclosing[4];

        public IFieldSplit[] SplitEnclosing { get; }

        public Point Position { get; set; }

        protected AbstractFieldObj()
            => SplitEnclosing = new IFieldSplit[9];

        public async Task UpdateFieldSplit(
            Func<IPacket> getEnterPacket = null,
            Func<IPacket> getLeavePacket = null
        )
        {
            if (Field == null)
            {
                await Task.WhenAll(SplitEnclosing
                    .Where(s => s != null)
                    .Select(s => s.Leave(this))
                );
                return;
            }

            var split = Field.GetSplit(Position);

            if (SplitCenter == split) return;

            var enclosingSplits = Field.GetEnclosingSplits(split);
            var oldSplits = SplitEnclosing
                .Where(s => s != null)
                .Except(enclosingSplits)
                .ToImmutableArray();
            var newSplits = enclosingSplits
                .Where(s => s != null)
                .Except(SplitEnclosing)
                .ToImmutableArray();

            enclosingSplits.CopyTo(SplitEnclosing, 0);

            await Task.WhenAll(oldSplits.Select(s => s.Leave(this, getLeavePacket)));
            await Task.WhenAll(newSplits.Select(s => s.Enter(this, getEnterPacket)));
        }

        public abstract IPacket GetEnterFieldPacket();
        public abstract IPacket GetLeaveFieldPacket();
    }
}