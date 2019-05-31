using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types
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
            packet.Encode<short>(_job);
            packet.Encode<byte>(_sp);
        }
    }
}