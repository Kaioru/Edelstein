using System;
using System.Collections.Generic;
using Edelstein.Service.Game.Conversations.Messages;
using MoreLinq;

namespace Edelstein.Service.Game.Conversations
{
    public abstract class AbstractSpeaker : ISpeaker
    {
        protected IConversationContext Context { get; }

        public abstract byte TypeID { get; }
        public abstract int TemplateID { get; }
        public abstract ScriptMessageParam Param { get; }

        public AbstractSpeaker(IConversationContext context)
            => Context = context;

        public ISpeaker AsQuiz() => new QuizSpeaker(Context, TemplateID, Param);
        public ISpeaker AsSpeedQuiz() => new SpeedQuizSpeaker(Context, TemplateID, Param);

        public byte Say(string[] text, int current = 0)
        {
            byte result = 0;

            while (current >= 0 && current < text.Length)
            {
                result = Say(text[current], current > 0, current < text.Length - 1);

                if (result == 0) current = Math.Max(0, --current);
                if (result == 1) current = Math.Min(text.Length, ++current);
            }

            return result;
        }

        public byte Say(string text = "", bool prev = false, bool next = true)
            => Context.Send<byte>(new SayScriptMessage(this, text, prev, next)).Result;

        public bool AskYesNo(string text = "")
            => Context.Send<byte>(new AskYesNoScriptMessage(this, text)).Result > 0;

        public bool AskAccept(string text = "")
            => Context.Send<byte>(new AskAcceptScriptMessage(this, text)).Result > 0;

        public string AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue)
            => Context.Send<string>(new AskTextScriptMessage(this, text, def, lenMin, lenMax)).Result;

        public string AskBoxText(string text = "", string def = "", short cols = 24, short rows = 4)
            => Context.Send<string>(new AskBoxTextScriptMessage(this, text, def, cols, rows)).Result;

        public int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue)
            => Context.Send<int>(new AskNumberScriptMessage(this, text, def, min, max)).Result;

        public int AskMenu(string text, IDictionary<int, string> options)
            => Context.Send<int>(new AskMenuScriptMessage(
                this,
                text,
                options
            )).Result;

        public byte AskAvatar(string text, int[] styles)
            => Context.Send<byte>(new AskAvatarScriptMessage(this, text, styles)).Result;

        public byte AskMembershopAvatar(string text, int[] styles)
            => Context.Send<byte>(new AskMembershopAvatarScriptMessage(this, text, styles)).Result;

        public int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0)
            => Context.Send<int>(new AskSlideMenuScriptMessage(
                this,
                type,
                selected,
                options
            )).Result;

        public string AskQuiz(
            string title = "", string quizText = "", string hintText = "",
            short minInput = 0, short maxInput = short.MaxValue, int remain = 15)
            => Context.Send<string>(new AskQuizScriptMessage(
                this,
                title, quizText, hintText,
                minInput, maxInput, remain
            )).Result;

        public int AskQuiz(ICollection<QuizOption> options, int remain = 15)
        {
            var correct = 0;

            options.ForEach(option =>
            {
                var result = AskQuiz(
                    option.Title,
                    option.QuizText,
                    option.HintText,
                    remain: remain
                );

                if (result.Equals(option.Validate)) correct++;
            });

            return correct;
        }

        public string AskSpeedQuiz(SpeedQuizType type, int answer, int correct = 0,
            int remain = 1, int remainTime = 15)
            => Context.Send<string>(new AskSpeedQuizScriptMessage(
                this,
                type, answer, correct,
                remain, remainTime
            )).Result;

        public int AskSpeedQuiz(ICollection<SpeedQuizOption> options, int remainTime = 15)
        {
            var completed = 0;
            var correct = 0;

            options.ForEach(option =>
            {
                var result = AskSpeedQuiz(
                    option.Type,
                    option.Answer,
                    correct,
                    options.Count - completed,
                    remainTime
                );

                if (result.Equals(option.Validate)) correct++;
                completed++;
            });

            return correct;
        }
    }
}