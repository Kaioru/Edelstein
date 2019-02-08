using Edelstein.Network.Packet;
using Edelstein.Service.Game.Conversations.Quiz;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskSpeedQuizScriptMessage : AbstractScriptMessage
    {
        public override ScriptMessageType Type => ScriptMessageType.AskSpeedQuiz;
        private readonly SpeedQuizType _type;
        private readonly int _answer;
        private readonly int _correct;
        private readonly int _remain;
        private readonly int _remainTime;

        public AskSpeedQuizScriptMessage(
            ISpeaker speaker,
            SpeedQuizType type,
            int answer,
            int correct,
            int remain,
            int remainTime
        ) : base(speaker)
        {
            _type = type;
            _answer = answer;
            _correct = correct;
            _remain = remain;
            _remainTime = remainTime;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<bool>(false);
            packet.Encode<int>((int) _type);
            packet.Encode<int>(_answer);
            packet.Encode<int>(_correct);
            packet.Encode<int>(_remain);
            packet.Encode<int>(_remainTime);
        }
    }
}