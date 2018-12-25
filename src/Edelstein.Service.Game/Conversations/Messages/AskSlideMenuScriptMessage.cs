using System.Collections.Generic;
using System.Linq;
using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskSlideMenuScriptMessage : AbstractScriptMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskSlideMenu;
        private readonly int _type;
        private readonly int _selected;
        private readonly string _text;
        private readonly IDictionary<int, string> _options;

        public AskSlideMenuScriptMessage(
            ISpeaker speaker,
            int type,
            int selected,
            IDictionary<int, string> options
        ) : base(speaker)
        {
            _type = type;
            _selected = selected;
            _text = string.Join("\r\n", options.Select(p => "#L" + p.Key + "#" + p.Value + "#l"));
            _options = options;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<int>(_type);
            packet.Encode<int>(_selected);
            packet.Encode<string>(_text);
        }

        public override bool Validate(object response)
        {
            if (response is int i)
                return _options.ContainsKey(i);
            return false;
        }
    }
}