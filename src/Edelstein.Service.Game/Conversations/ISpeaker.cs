using System.Collections.Generic;

namespace Edelstein.Service.Game.Conversations
{
    public interface ISpeaker
    {
        byte TypeID { get; }
        int TemplateID { get; }
        ScriptMessageParam Param { get; }

        byte Say(string[] text, int current = 0);
        byte Say(string text = "", bool prev = false, bool next = false);
        bool AskYesNo(string text = "");
        bool AskAccept(string text = "");
        string AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue);
        string AskBoxText(string text = "", string def = "", short cols = 24, short rows = 4);
        int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue);
        int AskMenu(string text, IDictionary<int, string> options);
        byte AskAvatar(string text, int[] styles);
        byte AskMembershopAvatar(string text, int[] styles);
        int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0);

        string AskQuiz(
            string title = "", string quizText = "", string hintText = "",
            short minInput = 0, short maxInput = short.MaxValue, int remain = 15);

        int AskQuiz(ICollection<QuizOption> options, int remain = 15);

        string AskSpeedQuiz(SpeedQuizType type, int answer, int correct = 0,
            int remain = 1, int remainTime = 15);

        int AskSpeedQuiz(ICollection<SpeedQuizOption> options, int remainTime = 15);
    }
}