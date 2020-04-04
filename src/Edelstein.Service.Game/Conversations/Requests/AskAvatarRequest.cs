using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;
using MoreLinq;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskAvatarRequest : AbstractConversationRequest<byte>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskAvatar;
        private readonly string _text;
        private readonly int[] _styles;

        public AskAvatarRequest(
            IConversationSpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker)
        {
            _text = text;
            _styles = styles;
        }

        public override bool Validate(IConversationResponse<byte> response)
            => response.Value <= _styles.Length;

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeString(_text);
            packet.EncodeByte((byte) _styles.Length);
            _styles.ForEach(s => packet.EncodeInt(s));
        }
    }
}