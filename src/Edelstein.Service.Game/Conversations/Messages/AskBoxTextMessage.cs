using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskBoxTextMessage : AbstractMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskBoxText;
        private readonly string _text;
        private readonly string _def;
        private readonly short _cols;
        private readonly short _rows;

        public AskBoxTextMessage(
            ISpeaker speaker,
            string text,
            string def,
            short cols,
            short rows
        ) : base(speaker)
        {
            _text = text;
            _def = def;
            _cols = cols;
            _rows = rows;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
            packet.Encode<string>(_def);
            packet.Encode<short>(_cols);
            packet.Encode<short>(_rows);
        }
    }
}