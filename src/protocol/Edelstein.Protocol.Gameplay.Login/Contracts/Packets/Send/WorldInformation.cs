using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record WorldInformation() : StructuredSendPacket((short)PacketSendOperation.WorldInformation)
{
    [FieldOrder(0)] public required byte ID { get; init; } = 0xFF;
    
    [FieldOrder(1)]
    [SerializeWhen(nameof(ID), 0xFF, ComparisonOperator.NotEqual)]
    public WorldInformationData? Data { get; init; }
}

public record WorldInformationData : StructuredBasePacket
{
    [FieldOrder(0)]
    public LPString Name { get; init; } = new();
    
    [FieldOrder(1)]
    public byte State { get; init; }
    
    [FieldOrder(2)]
    public LPString EventDesc { get; init; } = new();
    
    [FieldOrder(3)]
    public short EventEXP_WSE { get; init; }
    
    [FieldOrder(4)]
    public short EventDrop_WSE { get; init; }
    
    [FieldOrder(5)]
    public bool IsBlockCharCreation { get; init; }
    
    [FieldOrder(6)]
    public byte ChannelCount { get; init; }

    [FieldOrder(7)] 
    [FieldCount(nameof(ChannelCount))]
    public List<WorldInformationChannel> Channels { get; init; } = new();
    
    [FieldOrder(8)]
    public short BalloonCount { get; init; }
    
    [FieldOrder(9)] 
    [FieldCount(nameof(BalloonCount))]
    public List<WorldInformationBalloon> Balloons { get; init; } = new();
}

public record WorldInformationChannel : StructuredBasePacket
{
    [FieldOrder(0)] public LPString Name { get; init; } = new();
    [FieldOrder(1)] public int UserNo { get; init; }
    [FieldOrder(2)] public byte WorldID { get; init; }
    [FieldOrder(3)] public byte ChannelID { get; init; }
    [FieldOrder(4)] public bool IsAdultChannel { get; init; }
}

public record WorldInformationBalloon : StructuredBasePacket
{
    [FieldOrder(0)] public short X { get; init; }
    [FieldOrder(1)] public short Y { get; init; }
    [FieldOrder(2)] public LPString Message { get; init; } = new();
}
