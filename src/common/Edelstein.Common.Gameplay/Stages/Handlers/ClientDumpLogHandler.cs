using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Handlers
{
    public class ClientDumpLogHandler<TStage, TUser, TConfig> : AbstractPacketHandler<TStage, TUser>
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : ServerStageConfig
    {
        public override short Operation => (short)PacketRecvOperations.ClientDumpLog;

        public override Task Handle(TUser user, IPacketReader packet)
        {
            var callType = packet.ReadShort(); // 1 - CInPacket::Decode, 2 - ZTLException, 3 - CMSException
            var errorCode = packet.ReadInt();
            var backupBufferSize = packet.ReadShort();
            var backupBuffer = packet.ReadBytes((short)(backupBufferSize - 4));
            var sendSeq = packet.ReadUInt();

            var backupPacket = new UnstructuredIncomingPacket(backupBuffer);
            var operation = backupPacket.ReadShort();

            // TODO: handle this properly
            // $"Client exited with error code: {errorCode}, backup packet buffer operation 0x{operation:X} ({Enum.GetName((PacketSendOperations)operation)})");
            return Task.CompletedTask;
        }
    }
}
