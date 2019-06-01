using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Types
{
    public class IncPOPMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.IncPOPMessage;
        private readonly int _pop;

        public IncPOPMessage(int pop)
        {
            _pop = pop;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<int>(_pop);
        }
    }
}