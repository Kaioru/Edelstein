using System;

namespace Edelstein.Service.Game.Conversations
{
    public class QuizOption
    {
        public string Title { get; }
        public string QuizText { get; }
        public string HintText { get; }
        public string Validate { get; }
        public StringComparison Comparison { get; }

        public QuizOption(
            string title,
            string quizText,
            string hintText,
            string validate,
            StringComparison comparison
        )
        {
            Title = title;
            QuizText = quizText;
            HintText = hintText;
            Validate = validate;
            Comparison = comparison;
        }
    }
}