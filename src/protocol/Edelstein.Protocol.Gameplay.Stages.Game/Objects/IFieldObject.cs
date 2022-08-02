﻿using Edelstein.Protocol.Util.Buffers.Bytes;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldObject
{
    FieldObjectType Type { get; }

    int? ObjectID { get; set; }

    IField? Field { get; set; }
    IFieldSplit? FieldSplit { get; set; }
    IPoint2D Position { get; set; }

    IPacket GetEnterFieldPacket();
    IPacket GetLeaveFieldPacket();
}