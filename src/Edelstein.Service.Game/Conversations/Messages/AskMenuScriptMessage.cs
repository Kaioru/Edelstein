using System.Collections.Generic;
using System.Linq;
using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskMenuScriptMessage : AbstractScriptMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskMenu;
        private readonly string _text;
        private readonly IDictionary<int, string> _options;

        public AskMenuScriptMessage(
            ISpeaker speaker,
            string text,
            IDictionary<int, string> options
        ) : base(speaker)
        {
            _text = text + "\r\n#b" + string.Join("\r\n", options.Select(p => "#L" + p.Key + "#" + p.Value + "#l"));
            _options = options;
        }

        protected override void EncodeData(IPacket packet)
            => packet.Encode<string>(_text);

        public override bool Validate(object response)
        {
            if (response is int i)
                return _options.ContainsKey(i);
            return false;
        }
    }
}