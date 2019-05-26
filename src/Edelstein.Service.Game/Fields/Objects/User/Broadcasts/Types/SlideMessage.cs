using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Broadcasts.Types
{
    public class SlideMessage : AbstractBroadcastMessage
    {
        public override BroadcastMessageType Type => BroadcastMessageType.Slide;
        private readonly string _message;

        public SlideMessage(string message)
        {
            _message = message;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_message);
        }
    }
}