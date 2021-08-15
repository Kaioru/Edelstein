using Edelstein.Common.Gameplay.Handling;

namespace Edelstein.Common.Gameplay.Constants
{
    public static class ServerConstants
    {
        public static bool IsIgnoredLoggingPacket(int operation)
        {
            switch ((PacketRecvOperations)operation)
            {
                case PacketRecvOperations.AliveAck:
                case PacketRecvOperations.UserMove:
                case PacketRecvOperations.PetMove:
                case PacketRecvOperations.SummonedMove:
                case PacketRecvOperations.DragonMove:
                case PacketRecvOperations.MobMove:
                case PacketRecvOperations.NpcMove:
                    return true;
            }

            return false;
        }
    }
}