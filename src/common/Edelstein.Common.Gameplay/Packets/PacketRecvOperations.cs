namespace Edelstein.Common.Gameplay.Packets;

public enum PacketRecvOperations : short
{
    DUMMY_CODE = 100,

    BEGIN_SOCKET = 101,
    PermissionRequest = 103,
    LoginBasicInfo = 104,
    CheckLoginAuthInfo = 105,
    SelectWorld = 106,
    CheckSPWRequest = 107,
    SelectCharacter = 108,
    MigrateIn = 110,
    WorldInfoRequest = 114,
    LogoutWorld = 117,
    PrivateServerPacket = 134,
    AliveAck = 147,
    CheckHotfix = 152,
    CheckUserLimit = 157,
    WorldRequest = 160,
    EnableSPWRequest = 166,
    ChangeSPWRequest = 170,
    CheckNMCOServer = 171,

    UserMove = 190,
    UserChat = 203,

    NpcMove = 851
}
