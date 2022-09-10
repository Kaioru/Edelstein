namespace Edelstein.Common.Gameplay.Packets;

public enum PacketRecvOperations : short
{
    DUMMY_CODE = 100,

    BEGIN_SOCKET = 101,
    CheckLoginAuthInfo = 105,
    MigrateIn = 110,
    WorldInfoRequest = 114,
    LogoutWorld = 117,
    PrivateServerPacket = 134,
    AliveAck = 147,
    WorldRequest = 160,

    UserMove = 190,
    UserChat = 203,

    NpcMove = 851
}
