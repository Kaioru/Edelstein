using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;

public record StructuredScriptMessageAnswer : StructuredRecvPacket
{
    [FieldOrder(0)] public required ConversationMessageType Type { get; init; }
    [FieldOrder(1)] public required byte Action { get; init; }
    
    [FieldOrder(2)]
    [Subtype(nameof(Type), ConversationMessageType.Say, typeof(StructuredScriptMessageAnswerInfoSay))]
    [Subtype(nameof(Type), ConversationMessageType.SayImage, typeof(StructuredScriptMessageAnswerInfoSayImage))]
    [Subtype(nameof(Type), ConversationMessageType.AskYesNo, typeof(StructuredScriptMessageAnswerInfoAskYesNo))]
    [Subtype(nameof(Type), ConversationMessageType.AskText, typeof(StructuredScriptMessageAnswerInfoAskText))]
    [Subtype(nameof(Type), ConversationMessageType.AskNumber, typeof(StructuredScriptMessageAnswerInfoAskNumber))]
    [Subtype(nameof(Type), ConversationMessageType.AskMenu, typeof(StructuredScriptMessageAnswerInfoAskMenu))]
    [Subtype(nameof(Type), ConversationMessageType.AskQuiz, typeof(StructuredScriptMessageAnswerInfoAskQuiz))]
    [Subtype(nameof(Type), ConversationMessageType.AskSpeedQuiz, typeof(StructuredScriptMessageAnswerInfoAskSpeedQuiz))]
    [Subtype(nameof(Type), ConversationMessageType.AskAvatar, typeof(StructuredScriptMessageAnswerInfoAskAvatar))]
    [Subtype(nameof(Type), ConversationMessageType.AskMemberShopAvatar, typeof(StructuredScriptMessageAnswerInfoAskMemberShopAvatar))]
    [Subtype(nameof(Type), ConversationMessageType.AskPet, typeof(StructuredScriptMessageAnswerInfoAskPet))]
    [Subtype(nameof(Type), ConversationMessageType.AskPetAll, typeof(StructuredScriptMessageAnswerInfoAskPetAll))]
    [Subtype(nameof(Type), ConversationMessageType.AskAccept, typeof(StructuredScriptMessageAnswerInfoAskAccept))]
    [Subtype(nameof(Type), ConversationMessageType.AskBoxText, typeof(StructuredScriptMessageAnswerInfoAskBoxText))]
    [Subtype(nameof(Type), ConversationMessageType.AskSlideMenu, typeof(StructuredScriptMessageAnswerInfoAskSlideMenu))]
    [SubtypeDefault(typeof(StructuredScriptMessageAnswerInfo))]
    public required StructuredScriptMessageAnswerInfo? Info { get; init; }
}

public record StructuredScriptMessageAnswerInfo : StructuredBasePacket;

public record StructuredScriptMessageAnswerInfoAnswer<T> : StructuredScriptMessageAnswerInfo
{
    [FieldOrder(0)] public required T Answer { get; init; }
}

public record StructuredScriptMessageAnswerInfoSay : StructuredScriptMessageAnswerInfo;
public record StructuredScriptMessageAnswerInfoSayImage : StructuredScriptMessageAnswerInfo;
public record StructuredScriptMessageAnswerInfoAskYesNo : StructuredScriptMessageAnswerInfo;
public record StructuredScriptMessageAnswerInfoAskText : StructuredScriptMessageAnswerInfoAnswer<LPString>;
public record StructuredScriptMessageAnswerInfoAskNumber : StructuredScriptMessageAnswerInfoAnswer<int>;
public record StructuredScriptMessageAnswerInfoAskMenu : StructuredScriptMessageAnswerInfoAnswer<int>;
public record StructuredScriptMessageAnswerInfoAskQuiz : StructuredScriptMessageAnswerInfoAnswer<LPString>;
public record StructuredScriptMessageAnswerInfoAskSpeedQuiz : StructuredScriptMessageAnswerInfoAnswer<LPString>;
public record StructuredScriptMessageAnswerInfoAskAvatar : StructuredScriptMessageAnswerInfoAnswer<byte>;
public record StructuredScriptMessageAnswerInfoAskMemberShopAvatar : StructuredScriptMessageAnswerInfoAnswer<byte>;
public record StructuredScriptMessageAnswerInfoAskPet : StructuredScriptMessageAnswerInfoAnswer<byte>;
public record StructuredScriptMessageAnswerInfoAskPetAll : StructuredScriptMessageAnswerInfo;
public record StructuredScriptMessageAnswerInfoAskAccept : StructuredScriptMessageAnswerInfo;
public record StructuredScriptMessageAnswerInfoAskBoxText : StructuredScriptMessageAnswerInfoAnswer<LPString>;
public record StructuredScriptMessageAnswerInfoAskSlideMenu : StructuredScriptMessageAnswerInfoAnswer<int>;
