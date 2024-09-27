using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record MobMove : StructuredRecvPacket
{
    [FieldOrder(0)] public int ObjectID { get; init; }

    [FieldOrder(1)] public short MobCtrlSN { get; init; }
    
    [FieldOrder(2)] 
    [FieldBitLength(4)]
    public byte MobCtrlState { get; init; }
    
    [FieldOrder(3)] 
    [FieldBitLength(1)]
    public bool RiseByToss { get; init; }
    
    [FieldOrder(4)]
    [FieldBitLength(1)]
    public bool RushMove { get; init; }
    
    [FieldOrder(5)]
    [FieldBitLength(2)]
    public bool DirLeft { get; init; }
    
    [FieldOrder(6)] public byte Action { get; init; }
    
    [FieldOrder(7)] public int TargetInfo { get; init; }
    
    [FieldOrder(8)]
    public int MultiTargetForBallCount { get; init; }

    [FieldOrder(9)] 
    [FieldCount(nameof(MultiTargetForBallCount))]
    public List<MobMoveInfoMultiTargetForBall> MultiTargetForBall { get; init; } = new();
    
    [FieldOrder(10)]
    public int RandTimeForAreaAttackCount { get; init; }

    [FieldOrder(11)] 
    [FieldCount(nameof(RandTimeForAreaAttackCount))]
    public List<MobMoveInfoRandTimeForAreaAttack> RandTimeForAreaAttack { get; init; } = new();
    
    [FieldOrder(12)] 
    [FieldBitLength(4)]
    public bool IsCheatMobMoveRand { get; init; }
    
    [FieldOrder(13)] 
    [FieldBitLength(4)]
    public byte Active { get; init; }
    
    [FieldOrder(14)] public int HackedCode { get; init; }
    [FieldOrder(15)] public int TargetX { get; init; }
    [FieldOrder(16)] public int TargetY { get; init; }
    [FieldOrder(17)] public int HackedCodeCrc { get; init; }
    
    [FieldOrder(18)] public required StructuredMovePath Path { get; init; }
    
    [FieldOrder(19)] public bool Chasing { get; init; }
    [FieldOrder(20)] public bool Target { get; init; }
    [FieldOrder(21)] public bool ActiveChasing { get; init; }
    [FieldOrder(22)] public bool ActiveChasingHack { get; init; }
    [FieldOrder(23)] public int ActiveChasingDuration { get; init; }
}

public record MobMoveInfoMultiTargetForBall : StructuredBasePacket
{
    [FieldOrder(0)] public int X { get; init; }
    [FieldOrder(1)] public int Y { get; init; }
}

public record MobMoveInfoRandTimeForAreaAttack : StructuredBasePacket
{
    [FieldOrder(0)] public int Time { get; init; }
}
