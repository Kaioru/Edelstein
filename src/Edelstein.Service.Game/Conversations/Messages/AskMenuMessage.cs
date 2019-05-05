using System.Collections.Generic;
using System.Linq;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskMenuMessage : AbstractConversationMessage<int>
    {
        public override ConversationMessageType Type => ConversationMessageType.AskMenu;
        private readonly string _text;
        private readonly IDictionary<int, string> _options;

        public AskMenuMessage(
            ISpeaker speaker,
            string text,
            IDictionary<int, string> options
        ) : base(speaker)
        {
            _text =
                _text = text + "\r\n#b" + string.Join(
                            "\r\n",
                            options.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
                        );
            _options = options;
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
        }

        public override bool Validate(int response)
            => _options.ContainsKey(response);
    }
}