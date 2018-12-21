using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskSlideMenuMessage : AbstractMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskSlideMenu;
        private readonly int _type;
        private readonly int _selected;
        private readonly string _text;

        public AskSlideMenuMessage(
            ISpeaker speaker,
            int type,
            int selected,
            string text
        ) : base(speaker)
        {
            _type = type;
            _selected = selected;
            _text = text;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<int>(_type);
            packet.Encode<int>(_selected);
            packet.Encode<string>(_text);
        }
    }
}