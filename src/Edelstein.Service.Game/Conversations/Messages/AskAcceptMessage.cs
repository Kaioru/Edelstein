using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskAcceptMessage : AbstractMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskAccept;
        private readonly string _text;

        public AskAcceptMessage(
            ISpeaker speaker,
            string text
        ) : base(speaker)
            => _text = text;

        protected override void EncodeData(IPacket packet)
            => packet.Encode<string>(_text);
    }
}