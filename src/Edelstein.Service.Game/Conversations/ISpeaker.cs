using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversations
{
    public interface ISpeaker
    {
        byte TypeID { get; }
        int TemplateID { get; }
        ScriptMessageParam Param { get; }
        
        byte Say(string text = "", bool prev = false, bool next = false);
        bool AskYesNo(string text = "");
        bool AskAccept(string text = "");
        string AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue);
        string AskBoxText(string text = "", string def = "", short cols = 24, short rows = 4);
        int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue);

        Task<byte> SayAsync(string text = "", bool prev = false, bool next = false);
        Task<bool> AskYesNoAsync(string text = "");
        Task<bool> AskAcceptAsync(string text = "");
        Task<string> AskTextAsync(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue);
        Task<string> AskBoxTextAsync(string text = "", string def = "", short cols = 24, short rows = 4);
        Task<int> AskNumberAsync(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue);
    }
}