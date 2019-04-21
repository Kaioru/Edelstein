using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskNumberMessage : AbstractConversationMessage<int>
    {
        public override ConversationMessageType Type => ConversationMessageType.AskNumber;
        private readonly string _text;
        private readonly int _def;
        private readonly int _min;
        private readonly int _max;

        public AskNumberMessage(
            ISpeaker speaker,
            string text,
            int def,
            int min,
            int max
        ) : base(speaker)
        {
            _text = text;
            _def = def;
            _min = min;
            _max = max;
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
            packet.Encode<int>(_def);
            packet.Encode<int>(_min);
            packet.Encode<int>(_max);
        }

        public override bool Validate(int response)
            => response >= _min &&
               response <= _max;
    }
}