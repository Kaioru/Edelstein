using System;
using System.Collections.Generic;

namespace Edelstein.Service.Game.Conversations.Quiz
{
    public class QuizSpeaker : Speaker
    {
        private ICollection<QuizOption> _options;
        public int Total => _options.Count;

        public QuizSpeaker(
            IConversationContext context,
            int templateID = 9010000,
            ScriptMessageParam param = (ScriptMessageParam) 0
        ) : base(context, templateID, param)
            => _options = new List<QuizOption>();

        public void AddOption(
            string title,
            string validate,
            string quizText = "",
            string hintText = "",
            StringComparison comparison = StringComparison.OrdinalIgnoreCase
        )
            => _options.Add(new QuizOption(title, quizText, hintText, validate, comparison));

        public int Start(int remainTime = 15)
            => AskQuiz(_options, remainTime);
    }
}