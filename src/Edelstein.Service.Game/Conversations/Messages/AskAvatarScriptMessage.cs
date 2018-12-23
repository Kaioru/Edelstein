using Edelstein.Network.Packet;
using MoreLinq;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskAvatarScriptMessage : AbstractScriptMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskAvatar;
        private readonly string _text;
        private readonly int[] _styles;

        public AskAvatarScriptMessage(
            ISpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker)
        {
            _text = text;
            _styles = styles;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
            packet.Encode<byte>((byte) _styles.Length);
            _styles.ForEach(s => packet.Encode<int>(s));
        }

        public override bool Validate(object response)
        {
            if (response is byte b)
                return b <= _styles.Length;
            return false;
        }
    }
}