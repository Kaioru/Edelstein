using System.Collections.Generic;
using System.Linq;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskSlideMenuRequest : AbstractConversationRequest<int>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskSlideMenu;
        private readonly int _type;
        private readonly int _selected;
        private readonly string _text;
        private readonly IDictionary<int, string> _options;

        public AskSlideMenuRequest(
            IConversationSpeaker speaker,
            int type,
            int selected,
            IDictionary<int, string> options
        ) : base(speaker)
        {
            _type = type;
            _selected = selected;
            _text = string.Join(
                "\r\n",
                options.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
            );
            _options = options;
        }

        public override bool Validate(IConversationResponse<int> response)
            => _options.ContainsKey(response.Value);

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeInt(_type);
            packet.EncodeInt(_selected);
            packet.EncodeString(_text);
        }
    }
}