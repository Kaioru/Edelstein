using Edelstein.Common.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeaker : IConversationSpeaker
{
    private readonly IConversationContext _context;

    public ConversationSpeaker(
        IConversationContext context,
        int id = 9010000, ConversationSpeakerFlags flags = 0
    )
    {
        ID = id;
        Flags = flags;
        _context = context;
    }

    public int ID { get; }
    public ConversationSpeakerFlags Flags { get; }

    public byte Say(string text, bool prev = false, bool next = true) => 
        _context.Request(new SayRequest(this, text, prev, next)).Result;

    public byte Say(string[] text, int current = 0)
    {
        byte result = 0;

        while (current >= 0 && current < text.Length)
        {
            result = Say(text[current], current > 0);

            if (result == 0) current = Math.Max(0, --current);
            if (result != 1) continue;
            if (current == text.Length)
                break;
            current = Math.Min(text.Length, ++current);
        }

        return result;
    }

    public bool AskYesNo(string text) =>
        _context.Request(new AskYesNoRequest(this, text)).Result;

    public bool AskAccept(string text) =>
        _context.Request(new AskAcceptRequest(this, text)).Result;

    public string AskText(string text, string def = "", short lenMin = 0, short lenMax = short.MaxValue) =>
        _context.Request(new AskTextRequest(this, text, def, lenMin, lenMax)).Result;

    public string AskBoxText(string text, string def = "", short rows = 4, short cols = 24) =>
        _context.Request(new AskBoxTextRequest(this, text, def, rows, cols)).Result;

    public int AskNumber(string text, int def = 0, int min = int.MinValue, int max = int.MaxValue) =>
        _context.Request(new AskNumberRequest(this, text, def, min, max)).Result;

    public int AskMenu(string text, IDictionary<int, string> options) =>
        _context.Request(new AskMenuRequest(this, text, options)).Result;

    public byte AskAvatar(string text, int[] styles) =>
        _context.Request(new AskAvatarRequest(this, text, styles)).Result;

    public byte AskMemberShopAvatar(string text, int[] styles) =>
        _context.Request(new AskMemberShopAvatarRequest(this, text, styles)).Result;

    public int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0) =>
        _context.Request(new AskSlideMenuRequest(this, type, selected, options)).Result;
}
