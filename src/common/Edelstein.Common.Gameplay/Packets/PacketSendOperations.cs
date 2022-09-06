namespace Edelstein.Common.Gameplay.Packets;

public enum PacketSendOperations : short
{
    CheckPasswordResult = 0,
    WorldInformation = 1,
    LatestConnectedWorld = 2,
    AliveReq = 18
}
