using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskQuizMessage : AbstractMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskQuiz;
        private readonly string _title;
        private readonly string _quizText;
        private readonly string _hintText;
        private readonly int _minInput;
        private readonly int _maxInput;
        private readonly int _remain;

        public AskQuizMessage(
            ISpeaker speaker,
            string title,
            string quizText,
            string hintText,
            int minInput,
            int maxInput,
            int remain
        ) : base(speaker)
        {
            _title = title;
            _quizText = quizText;
            _hintText = hintText;
            _minInput = minInput;
            _maxInput = maxInput;
            _remain = remain;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<bool>(false);
            packet.Encode<string>(_title);
            packet.Encode<string>(_quizText);
            packet.Encode<string>(_hintText);
            packet.Encode<int>(_minInput);
            packet.Encode<int>(_maxInput);
            packet.Encode<int>(_remain);
        }
    }
}