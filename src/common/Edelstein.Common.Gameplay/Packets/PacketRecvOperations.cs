﻿namespace Edelstein.Common.Gameplay.Packets;

public enum PacketRecvOperations : short
{
    DUMMY_CODE = 100,

    BEGIN_SOCKET = 101,

    MigrateIn = 110,
    AliveAck = 147
}
