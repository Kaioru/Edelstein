using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class SayMessage : AbstractMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.Say;
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

        protected override void EncodeData(IPacket packet)
        {
            if (Speaker.Param.HasFlag(ScriptMessageParam.NPCReplacedByNPC))
                packet.Encode<int>(Speaker.TemplateID);
            packet.Encode<string>(_text);
            packet.Encode<bool>(_prev);
            packet.Encode<bool>(_next);
        }
    }
}