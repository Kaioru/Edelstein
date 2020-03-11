using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    public interface IConversationSpeaker
    {
        IConversationContext Context { get; }

        int TemplateID { get; }
        ConversationSpeakerType Type { get; }

        IConversationSpeaker AsSpeaker(int templateID, ConversationSpeakerType type = 0);
        IConversationSpeech GetSpeech(string text);

        byte Say(IConversationSpeech[] text, int current = 0);
        byte Say(string text = "", bool prev = false, bool next = true);
        bool AskYesNo(string text = "");
        bool AskAccept(string text = "");
        string AskText(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue);
        string AskBoxText(string text = "", string def = "", short cols = 24, short rows = 4);
        int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue);
        int AskMenu(string text, IDictionary<int, string> options);
        byte AskAvatar(string text, int[] styles);
        byte AskMemberShopAvatar(string text, int[] styles);
        int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0);

        Task<byte> SayAsync(IConversationSpeech[] text, int current = 0);
        Task<byte> SayAsync(string text = "", bool prev = false, bool next = true);
        Task<bool> AskYesNoAsync(string text = "");
        Task<bool> AskAcceptAsync(string text = "");
        Task<string> AskTextAsync(string text = "", string def = "", short lenMin = 0, short lenMax = short.MaxValue);
        Task<string> AskBoxTextAsync(string text = "", string def = "", short cols = 24, short rows = 4);
        Task<int> AskNumberAsync(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue);
        Task<int> AskMenuAsync(string text, IDictionary<int, string> options);
        Task<byte> AskAvatarAsync(string text, int[] styles);
        Task<byte> AskMemberShopAvatarAsync(string text, int[] styles);
        Task<int> AskSlideMenuAsync(IDictionary<int, string> options, int type = 0, int selected = 0);
    }
}