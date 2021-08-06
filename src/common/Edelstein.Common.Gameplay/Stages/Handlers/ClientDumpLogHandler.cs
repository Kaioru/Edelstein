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
        where TConfig : ServerStageConfig
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

            _stage.Logger.LogError(
                $"Client (Account ID: {user?.Account?.ID.ToString() ?? "(none)"}, Character ID: {user?.Character?.ID.ToString() ?? "(none)"}) " +
                $"exited with error code: {errorCode} (call type: {callType}) " +
                (_stage.Logger.IsEnabled(LogLevel.Debug)
                    ? $"with packet buffer of: {BitConverter.ToString(backupBuffer).Replace("-", " ")}"
                    : ""));
            return Task.CompletedTask;
        }
    }
}
