using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record WorldInformation() : StructuredSendPacket(PacketSendOperation.WorldInformation)
{
    [FieldOrder(0)] public required byte ID { get; init; }
    
    [FieldOrder(1)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public LPString Name { get; init; } = new();
    
    [FieldOrder(2)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public byte State { get; init; }
    
    [FieldOrder(3)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public LPString EventDesc { get; init; } = new();
    
    [FieldOrder(4)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public short EventEXP_WSE { get; init; }
    
    [FieldOrder(5)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public short EventDrop_WSE { get; init; }
    
    [FieldOrder(6)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public bool IsBlockCharCreation { get; init; }
    
    [FieldOrder(7)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public byte ChannelCount { get; init; }

    [FieldOrder(8)] 
    [FieldLength(nameof(ChannelCount))]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public List<WorldInformationChannel> Channels { get; init; } = new();
    
    [FieldOrder(9)]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
    public byte BalloonCount { get; init; }
    
    [FieldOrder(10)] 
    [FieldLength(nameof(BalloonCount))]
    [SerializeWhen(nameof(ID), 0, ComparisonOperator.NotEqual)]
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
