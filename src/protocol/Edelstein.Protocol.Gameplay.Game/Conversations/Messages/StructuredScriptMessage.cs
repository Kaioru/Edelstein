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
