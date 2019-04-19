using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Login.Sockets
{
    public partial class LoginSocket : AbstractMigrateableSocket<LoginServiceInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public WvsLogin WvsLogin { get; }

        public Account Account { get; set; }
        public AccountData AccountData { get; set; }
        public Character Character { get; set; }

        public LoginSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsLogin service
        ) : base(channel, seqSend, seqRecv, service)
        {
            WvsLogin = service;
        }

        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();
            return operation switch {
                RecvPacketOperations.CheckPassword => OnCheckPassword(packet),
                RecvPacketOperations.WorldInfoRequest => OnWorldInfoRequest(packet),
                RecvPacketOperations.SetGender => OnSetGender(packet),
                RecvPacketOperations.CheckPinCode => OnCheckPinCode(packet),
                RecvPacketOperations.WorldRequest => OnWorldInfoRequest(packet),
                _ => Task.Run(() => Logger.Warn($"Unhandled packet operation {operation}"))
                };
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Socket caught exception");
            return Task.CompletedTask;
        }

        public override async Task OnDisconnect()
        {
            if (Account == null) return;
            var state = (await WvsLogin.AccountStateCache.GetAsync<MigrationState>(Account.ID.ToString())).Value;
            if (state != MigrationState.Migrating)
            {
                await WvsLogin.AccountStateCache.RemoveAsync(Account.ID.ToString());
            }
        }
    }
}