using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Network;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Handlers
{
    public class ClientDumpLogHandler<TStage, TUser, TConfig> : AbstractPacketHandler<TStage, TUser>
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : ServerStageInfo
    {
        public override short Operation => (short)PacketRecvOperations.ClientDumpLog;
        private readonly TStage _stage;

        public ClientDumpLogHandler(TStage stage)
            => _stage = stage;

        public override Task Handle(TUser user, IPacketReader packet)
        {
            var callType = packet.ReadShort(); // 1 - CInPacket::Decode, 2 - ZTLException, 3 - CMSException
            var errorCode = packet.ReadInt();
            var backupBufferSize = packet.ReadShort();
            var backupBuffer = packet.ReadBytes(backupBufferSize);

            var backupPacket = new UnstructuredIncomingPacket(backupBuffer);
            var seqSend = backupPacket.ReadUInt();
            var operation = backupPacket.ReadShort();
            var payload = backupPacket.ReadBytes((short)backupPacket.Available);

            _stage.Logger.LogError(
                $"Client (Account ID: {user?.Account?.ID.ToString() ?? "(none)"}, Character ID: {user?.Character?.ID.ToString() ?? "(none)"}) " +
                $"exited with error code: {errorCode} (call type: {callType}) " +
                $"from operation 0x{operation:X} ({Enum.GetName((PacketSendOperations)operation)}) " +
                (_stage.Logger.IsEnabled(LogLevel.Debug)
                    ? $"with payload: {BitConverter.ToString(payload).Replace("-", " ")} (SeqSend: {seqSend})"
                    : ""));
            return Task.CompletedTask;
        }
    }
}
