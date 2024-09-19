using System.Collections.Generic;
using Edelstein.Common.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeaker(
    IConversationContext context,
    ConversationSpeakerType type = ConversationSpeakerType.User, 
    int templateID = 9010000,
    ConversationSpeakerParam @params = 0
) : IConversationSpeaker
{
    public ConversationSpeakerType Type { get; } = type;
    public int TemplateID { get; } = templateID;
    public ConversationSpeakerParam Params { get; } = @params;

    public byte Say(string text, bool prev = false, bool next = true)
        => context.Ask(new ConversationMessageSay(
            this,
            text,
            prev,
            next
        )).Result;
    
    public byte SayImage(string[] path)
        => context.Ask(new ConversationMessageSayImage(
            this,
            path
        )).Result;
    
    public bool AskYesNo(string text)
        => context.Ask(new ConversationMessageAskYesNo(
            this,
            text
        )).Result > 0;
    
    public bool AskAccept(string text)
        => context.Ask(new ConversationMessageAskAccept(
            this,
            text
        )).Result > 0;
    
    public string AskText(string text, string def = "", short lenMin = 0, short lenMax = short.MaxValue) 
        => context.Ask(new ConversationMessageAskText(
            this,
            text,
            def,
            lenMin,
            lenMax
        )).Result;
    
    public string AskBoxText(string text, string def = "", short rows = 4, short cols = 24)
        => context.Ask(new ConversationMessageAskBoxText(
            this,
            text,
            def,
            cols,
            rows
        )).Result;

    public int AskNumber(string text, int def = 0, int min = int.MinValue, int max = int.MaxValue) 
        => context.Ask(new ConversationMessageAskNumber(
            this,
            text,
            def,
            min,
            max
        )).Result;
    
    public int AskMenu(string text, IDictionary<int, string> options) 
        => context.Ask(new ConversationMessageAskMenu(
            this,
            text,
            options
        )).Result;
    
    public byte AskAvatar(string text, int[] styles)  
        => context.Ask(new ConversationMessageAskAvatar(
            this,
            text,
            styles
        )).Result;
    
    public byte AskMemberShopAvatar(string text, int[] styles) 
        => context.Ask(new ConversationMessageAskMemberShopAvatar(
            this,
            text,
            styles
        )).Result;

    public int AskSlideMenu(IDictionary<int, string> options, int type = 0, int selected = 0)
        => context.Ask(new ConversationMessageAskSlideMenu(
            this,
            type,
            selected,
            options
        )).Result;
}
