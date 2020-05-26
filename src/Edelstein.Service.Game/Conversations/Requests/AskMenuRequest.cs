using System.Collections.Generic;
using System.Linq;
using Edelstein.Core.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskMenuRequest : AbstractConversationRequest<int>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskMenu;
        private readonly string _text;
        private readonly IDictionary<int, string> _options;

        public AskMenuRequest(
            IConversationSpeaker speaker,
            string text,
            IDictionary<int, string> options
        ) : base(speaker)
        {
            _text = text + "\r\n#b" + string.Join(
                        "\r\n",
                        options.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
                    );
            _options = options;
        }

        public override bool Validate(IConversationResponse<int> response)
            => _options.ContainsKey(response.Value);

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeString(_text);
        }
    }
}