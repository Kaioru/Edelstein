using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversation
{
    public interface ISpeaker
    {
        byte TypeID { get; }
        int TemplateID { get; }
        ScriptMessageParam Param { get; }

        Task<byte> Say(string text = "", bool prev = false, bool next = false);
        Task<bool> AskYesNo(string text = "");
        Task<bool> AskAccept(string text = "");
        Task<string> AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue);
        Task<string> AskBoxText(string text = "", string def = "", short cols = 24, short rows = 4);
        Task<int> AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue);
    }
}