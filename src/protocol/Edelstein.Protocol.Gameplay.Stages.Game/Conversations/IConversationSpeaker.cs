using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations
{
    public interface IConversationSpeaker
    {
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
    }
}
