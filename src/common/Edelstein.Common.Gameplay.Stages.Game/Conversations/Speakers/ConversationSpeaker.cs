using Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Speakers;

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

    public bool AskYesNo(string text) => throw new NotImplementedException();

    public bool AskAccept(string text) => throw new NotImplementedException();

    public string AskText(string text, string def = "", short lenMin = 0, short lenMax = short.MaxValue) =>
        throw new NotImplementedException();

    public string AskBoxText(string text, string def = "", short rows = 4, short cols = 24) =>
        throw new NotImplementedException();

    public int AskNumber(string text, int def = 0, int min = int.MinValue, int max = int.MaxValue) =>
        throw new NotImplementedException();

    public int AskMenu(string text, IDictionary<int, string> options) => throw new NotImplementedException();

    public byte AskAvatar(string text, int[] styles) => throw new NotImplementedException();

    public byte AskMemberShopAvatar(string text, int[] styles) => throw new NotImplementedException();

    public int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0) =>
        throw new NotImplementedException();
}
