using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskTextScriptMessage : AbstractScriptMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskText;
        private readonly string _text;
        private readonly string _def;
        private readonly short _lenMin;
        private readonly short _lenMax;

        public AskTextScriptMessage(
            ISpeaker speaker,
            string text,
            string def,
            short lenMin,
            short lenMax
        ) : base(speaker)
        {
            _text = text;
            _def = def;
            _lenMin = lenMin;
            _lenMax = lenMax;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
            packet.Encode<string>(_def);
            packet.Encode<short>(_lenMin);
            packet.Encode<short>(_lenMax);
        }

        public override bool Validate(object response)
        {
            if (response is string str)
            {
                return str.Length >= _lenMin &&
                       str.Length <= _lenMax;
            }

            return false;
        }
    }
}