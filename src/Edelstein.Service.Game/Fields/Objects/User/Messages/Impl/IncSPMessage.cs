using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl
{
    public class IncSPMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.IncSPMessage;
        private readonly short _job;
        private readonly byte _sp;

        public IncSPMessage(short job, byte sp)
        {
            _job = job;
            _sp = sp;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.EncodeShort(_job);
            packet.EncodeByte(_sp);
        }
    }
}