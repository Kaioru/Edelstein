using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public record StructuredScriptMessage() : StructuredSendPacket((short)PacketSendOperation.ScriptMessage)
{
    [FieldOrder(0)] public required ConversationSpeakerType SpeakerType { get; init; }
    [FieldOrder(1)] public required int SpeakerTemplateID { get; init; }
    
    [FieldOrder(2)] public required ConversationMessageType Type { get; init; }
    [FieldOrder(3)] public required byte Params { get; init; }
    
    [FieldOrder(4)]
    [Subtype(nameof(Type), ConversationMessageType.Say, typeof(StructuredScriptMessageInfoSay))]
    [Subtype(nameof(Type), ConversationMessageType.SayImage, typeof(StructuredScriptMessageInfoSayImage))]
    [Subtype(nameof(Type), ConversationMessageType.AskYesNo, typeof(StructuredScriptMessageInfoAskYesNo))]
    [Subtype(nameof(Type), ConversationMessageType.AskAccept, typeof(StructuredScriptMessageInfoAskAccept))]
    [Subtype(nameof(Type), ConversationMessageType.AskText, typeof(StructuredScriptMessageInfoAskText))]
    [Subtype(nameof(Type), ConversationMessageType.AskBoxText, typeof(StructuredScriptMessageInfoAskBoxText))]
    [Subtype(nameof(Type), ConversationMessageType.AskNumber, typeof(StructuredScriptMessageInfoAskNumber))]
    [Subtype(nameof(Type), ConversationMessageType.AskMenu, typeof(StructuredScriptMessageInfoAskMenu))]
    [Subtype(nameof(Type), ConversationMessageType.AskAvatar, typeof(StructuredScriptMessageInfoAskAvatar))]
    [Subtype(nameof(Type), ConversationMessageType.AskMemberShopAvatar, typeof(StructuredScriptMessageInfoAskMemberShopAvatar))]
    [Subtype(nameof(Type), ConversationMessageType.AskSlideMenu, typeof(StructuredScriptMessageInfoAskSlideMenu))]
    [SubtypeDefault(typeof(StructuredScriptMessageInfo))]
    public required StructuredScriptMessageInfo Info { get; init; }
}

public record StructuredScriptMessageInfo : StructuredBasePacket;

public record StructuredScriptMessageInfoSay : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
    [FieldOrder(1)] public required bool Prev { get; init; }
    [FieldOrder(2)] public required bool Next { get; init; }
}

public record StructuredScriptMessageInfoSayImage : StructuredScriptMessageInfo
{
    [FieldOrder(0)]
    public int Count { get; init; }
    
    [FieldOrder(1)]
    [FieldCount(nameof(Count))]
    public required string[] Path { get; init; }
}

public record StructuredScriptMessageInfoAskYesNo : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
}

public record StructuredScriptMessageInfoAskAccept : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
}

public record StructuredScriptMessageInfoAskText : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
    [FieldOrder(1)] public required LPString Default { get; init; }
    [FieldOrder(2)] public required short LengthMin { get; init; }
    [FieldOrder(3)] public required short LengthMax { get; init; }
}

public record StructuredScriptMessageInfoAskBoxText : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
    [FieldOrder(1)] public required LPString Default { get; init; }
    [FieldOrder(2)] public required short Col { get; init; }
    [FieldOrder(3)] public required short Row { get; init; }
}

public record StructuredScriptMessageInfoAskNumber : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
    [FieldOrder(1)] public required int Default { get; init; }
    [FieldOrder(2)] public required int Min { get; init; }
    [FieldOrder(3)] public required int Max { get; init; }
}

public record StructuredScriptMessageInfoAskMenu : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
}

public record StructuredScriptMessageInfoAskAvatar : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
    [FieldOrder(1)] public byte Count { get; init; }
    
    [FieldOrder(2)] 
    [FieldCount(nameof(Count))]
    public required int[] Style { get; init; }
}

public record StructuredScriptMessageInfoAskMemberShopAvatar : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required LPString Text { get; init; }
    [FieldOrder(1)] public byte Count { get; init; }
    
    [FieldOrder(2)] 
    [FieldCount(nameof(Count))]
    public required int[] Style { get; init; }
}

public record StructuredScriptMessageInfoAskSlideMenu : StructuredScriptMessageInfo
{
    [FieldOrder(0)] public required int Type { get; init; }
    [FieldOrder(1)] public required int Selected { get; init; }
    [FieldOrder(2)] public required LPString Text { get; init; }
}


