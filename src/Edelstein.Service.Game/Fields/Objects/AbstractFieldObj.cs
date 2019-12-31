using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Fields.Objects
{
    public abstract class AbstractFieldObj : IFieldObj
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
        public abstract FieldObjType Type { get; }

        public int ID { get; set; }

        public IField Field { get; set; }
        public IFieldSplit FieldSplit { get; set; }

        public Point Position { get; set; }

        protected AbstractFieldObj()
        {
        }

        public async Task UpdateFieldSplit(
            Func<IPacket> getEnterPacket = null,
            Func<IPacket> getLeavePacket = null
        )
        {
            if (Field == null)
            {
                if (FieldSplit != null) await FieldSplit.Leave(this);
                return;
            }

            var split = Field.GetSplit(Position);

            if (FieldSplit != split)
            {
                await split.Enter(this, FieldSplit, getEnterPacket, getLeavePacket);
                Logger.Debug($"Migrated {Type} {ID} to field split (col: {split.Col}, row: {split.Row})");
            }
        }

        public abstract IPacket GetEnterFieldPacket();
        public abstract IPacket GetLeaveFieldPacket();

        public Task BroadcastPacket(IPacket packet)
            => Field.BroadcastPacket(this, packet);
    }
}