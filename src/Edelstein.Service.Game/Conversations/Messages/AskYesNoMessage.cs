using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskYesNoMessage : AbstractConversationMessage<bool>
    {
        public override ConversationMessageType Type => ConversationMessageType.AskYesNo;
        private readonly string _text;

        public AskYesNoMessage(
            ISpeaker speaker,
            string text
        ) : base(speaker)
        {
            _text = text;
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
        }

        public override bool Validate(bool response)
            => true;
    }
}