using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class SayMessage : AbstractConversationMessage<byte>
    {
        public override ConversationMessageType Type => ConversationMessageType.Say;
        private readonly string _text;
        private readonly bool _prev;
        private readonly bool _next;

        public SayMessage(
            ISpeaker speaker,
            string text,
            bool prev,
            bool next
        ) : base(speaker)
        {
            _text = text;
            _prev = prev;
            _next = next;
        }

        public override void EncodeData(IPacket packet)
        {
            if (Speaker.ParamType.HasFlag(SpeakerParamType.NPCReplacedByNPC))
                packet.Encode<int>(Speaker.TemplateID);
            packet.Encode<string>(_text);
            packet.Encode<bool>(_prev);
            packet.Encode<bool>(_next);
        }

        public override bool Validate(byte response)
            => true;
    }
}