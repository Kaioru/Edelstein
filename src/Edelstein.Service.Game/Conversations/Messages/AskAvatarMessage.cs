using Edelstein.Network.Packets;
using MoreLinq;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskAvatarMessage : AbstractConversationMessage<byte>
    {
        public override ConversationMessageType Type => ConversationMessageType.AskAvatar;
        private readonly string _text;
        private readonly int[] _styles;

        public AskAvatarMessage(
            ISpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker)
        {
            _text = text;
            _styles = styles;
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
            packet.Encode<byte>((byte) _styles.Length);
            _styles.ForEach(s => packet.Encode<int>(s));
        }

        public override bool Validate(byte response)
            => response <= _styles.Length;
    }
}