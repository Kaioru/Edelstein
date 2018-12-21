using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskMenuMessage : AbstractMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskMenu;
        private readonly string _text;

        public AskMenuMessage(
            ISpeaker speaker,
            string text
        ) : base(speaker)
            => _text = text;

        protected override void EncodeData(IPacket packet)
            => packet.Encode<string>(_text);
    }
}