using System.Linq;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

public static class ConversationMessageExtensions
{
    public static StructuredScriptMessage ToStructured<T>(this IConversationMessage<T> message)
        => new()
        {
            SpeakerType = message.Speaker.Type,
            SpeakerTemplateID = message.Speaker.TemplateID,
            Type = message.Type,
            Params = (byte)message.Speaker.Params,
            Info = message switch
            {
                ConversationMessageSay say => new StructuredScriptMessageInfoSay
                {
                    Text = new LPString(say.Text),
                    Next = say.Next,
                    Prev = say.Prev
                },
                ConversationMessageSayImage sayImage => new StructuredScriptMessageInfoSayImage
                {
                    Path = sayImage.Path
                },
                ConversationMessageAskYesNo askYesNo => new StructuredScriptMessageInfoAskYesNo
                {
                    Text = new LPString(askYesNo.Text)
                },
                ConversationMessageAskText askText => new StructuredScriptMessageInfoAskText
                {
                    Text = new LPString(askText.Text),
                    Default = new LPString(askText.Default),
                    LengthMin = askText.LengthMin,
                    LengthMax = askText.LengthMax
                },
                ConversationMessageAskNumber askNumber => new StructuredScriptMessageInfoAskNumber
                {
                    Text = new LPString(askNumber.Text),
                    Default = askNumber.Default,
                    Min = askNumber.Min,
                    Max = askNumber.Max
                },
                ConversationMessageAskMenu askMenu => new StructuredScriptMessageInfoAskMenu
                {
                    Text = new LPString(askMenu.Text + "\r\n#b" + string.Join(
                        "\r\n",
                        askMenu.Menu.Select(p => "#L" + p.Key + "#" + p.Value + "#l")
                    ))
                },
                ConversationMessageAskAvatar askAvatar => new StructuredScriptMessageInfoAskAvatar
                {
                    Text = new LPString(askAvatar.Text),
                    Style = askAvatar.Styles
                },
                ConversationMessageAskMemberShopAvatar askMemberShop => new StructuredScriptMessageInfoAskMemberShopAvatar
                {
                    Text = new LPString(askMemberShop.Text),
                    Style = askMemberShop.Styles
                },
                ConversationMessageAskAccept askAccept => new StructuredScriptMessageInfoAskAccept
                {
                    Text = new LPString(askAccept.Text)
                },
                ConversationMessageAskBoxText askBoxText => new StructuredScriptMessageInfoAskBoxText
                {
                    Text = new LPString(askBoxText.Text),
                    Default = new LPString(askBoxText.Default),
                    Col = askBoxText.Col,
                    Row = askBoxText.Row
                },
                ConversationMessageAskSlideMenu askSlideMenu => new StructuredScriptMessageInfoAskSlideMenu
                {
                    Type = askSlideMenu.SlideMenuType,
                    Selected = askSlideMenu.Selected,
                    Text = new LPString(string.Join(
                        string.Empty,
                        askSlideMenu.Menu.Select(p => "#" + p.Key + "#" + p.Value))
                    )
                },
                _ => new StructuredScriptMessageInfo()
            }
        };
}
