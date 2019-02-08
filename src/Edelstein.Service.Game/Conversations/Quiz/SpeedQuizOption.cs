using System;

namespace Edelstein.Service.Game.Conversations.Quiz
{
    public class SpeedQuizOption
    {
        public SpeedQuizType Type { get; }
        public int Answer { get; }
        public string Validate { get; }
        public StringComparison Comparison { get; }

        public SpeedQuizOption(SpeedQuizType type, int answer, string validate, StringComparison comparison)
        {
            Type = type;
            Answer = answer;
            Validate = validate;
            Comparison = comparison;
        }
    }
}