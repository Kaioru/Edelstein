using System;
using System.Collections.Generic;

namespace Edelstein.Service.Game.Conversations.Quiz
{
    public class SpeedQuizSpeaker : Speaker
    {
        private ICollection<SpeedQuizOption> _options;
        public int Total => _options.Count;

        public SpeedQuizSpeaker(
            IConversationContext context,
            int templateID = 9010000,
            ScriptMessageParam param = (ScriptMessageParam) 0
        ) : base(context, templateID, param)
            => _options = new List<SpeedQuizOption>();

        public void AddOption(
            SpeedQuizType type, int answer, string validate,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase
        )
            => _options.Add(new SpeedQuizOption(type, answer, validate, comparison));

        public int Start(int remainTime = 15)
            => AskSpeedQuiz(_options, remainTime);
    }
}