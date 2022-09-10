namespace Edelstein.Common.Gameplay.Packets;

public enum PacketSendOperations : short
{
    CheckPasswordResult = 0,
    WorldInformation = 1,
    LatestConnectedWorld = 2,
    SelectWorldResult = 6,
    SelectCharacterResult = 7,
    DeleteCharacterResult = 12,
    AliveReq = 18,
    PrivateServerPacket = 23,
    ChangeSPWResult = 24,
    ApplyHotfix = 36,
    CheckUserLimitResult = 38,
    CheckNMCOServerResult = 47,

    SetField = 428,

    UserEnterField = 516,
    UserLeaveField = 517,

    UserChat = 518,
    UserMove = 633,

    NpcEnterField = 984,
    NpcLeaveField = 985,
    NpcChangeController = 987,
    NpcMove = 988
}
