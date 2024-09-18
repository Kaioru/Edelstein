using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public record StructuredScriptMessageAnswer : StructuredRecvPacket
{
    [FieldOrder(0)] public required ConversationMessageType Type { get; init; }
    
    [FieldOrder(1)]
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
    public required StructuredScriptMessageAnswerInfo Info { get; init; }
}

public record StructuredScriptMessageAnswerInfo : StructuredBasePacket;

public record StructuredScriptMessageAnswerInfoStatus : StructuredScriptMessageAnswerInfo
{
    [FieldOrder(0)] public required byte Status { get; init; }
}

public record StructuredScriptMessageAnswerInfoAnswer<T> : StructuredScriptMessageAnswerInfoStatus
{
    [FieldOrder(0)] public required T Answer { get; init; }
}

public record StructuredScriptMessageAnswerInfoQuiz : StructuredScriptMessageAnswerInfo
{
    [FieldOrder(0)] public required LPString Answer { get; init; }
}

public record StructuredScriptMessageAnswerInfoSay : StructuredScriptMessageAnswerInfoStatus;
public record StructuredScriptMessageAnswerInfoSayImage : StructuredScriptMessageAnswerInfoStatus;
public record StructuredScriptMessageAnswerInfoAskYesNo : StructuredScriptMessageAnswerInfoStatus;
public record StructuredScriptMessageAnswerInfoAskText : StructuredScriptMessageAnswerInfoAnswer<LPString>;
public record StructuredScriptMessageAnswerInfoAskNumber : StructuredScriptMessageAnswerInfoAnswer<int>;
public record StructuredScriptMessageAnswerInfoAskMenu : StructuredScriptMessageAnswerInfoAnswer<int>;
public record StructuredScriptMessageAnswerInfoAskQuiz : StructuredScriptMessageAnswerInfoQuiz;
public record StructuredScriptMessageAnswerInfoAskSpeedQuiz : StructuredScriptMessageAnswerInfoQuiz;
public record StructuredScriptMessageAnswerInfoAskAvatar : StructuredScriptMessageAnswerInfoAnswer<byte>;
public record StructuredScriptMessageAnswerInfoAskMemberShopAvatar : StructuredScriptMessageAnswerInfoAnswer<byte>;
public record StructuredScriptMessageAnswerInfoAskPet : StructuredScriptMessageAnswerInfoAnswer<byte>;
public record StructuredScriptMessageAnswerInfoAskPetAll : StructuredScriptMessageAnswerInfoStatus;
public record StructuredScriptMessageAnswerInfoAskAccept : StructuredScriptMessageAnswerInfoStatus;
public record StructuredScriptMessageAnswerInfoAskBoxText : StructuredScriptMessageAnswerInfoAnswer<LPString>;
public record StructuredScriptMessageAnswerInfoAskSlideMenu : StructuredScriptMessageAnswerInfoAnswer<int>;
