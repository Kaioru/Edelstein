using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskYesNoScriptMessage : AbstractScriptMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskYesNo;
        private readonly string _text;

        public AskYesNoScriptMessage(
            ISpeaker speaker,
            string text
        ) : base(speaker)
            => _text = text;

        protected override void EncodeData(IPacket packet)
            => packet.Encode<string>(_text);
    }
}