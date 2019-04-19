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

namespace Edelstein.Service.Login.Services
{
    public partial class LoginSocket : AbstractMigrateableSocket<LoginServiceInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public LoginService Service { get; }

        public Account Account { get; set; }
        public AccountData AccountData { get; set; }
        public Character Character { get; set; }

        public LoginSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            LoginService service
        ) : base(channel, seqSend, seqRecv, service)
        {
            Service = service;
        }

        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();
            return operation switch {
                RecvPacketOperations.CheckPassword => OnCheckPassword(packet),
                RecvPacketOperations.WorldInfoRequest => OnWorldInfoRequest(packet),
                RecvPacketOperations.SelectWorld => OnSelectWorld(packet),
                RecvPacketOperations.CheckUserLimit => OnCheckUserLimit(packet),
                RecvPacketOperations.SetGender => OnSetGender(packet),
                RecvPacketOperations.CheckPinCode => OnCheckPinCode(packet),
                RecvPacketOperations.WorldRequest => OnWorldInfoRequest(packet),
                RecvPacketOperations.AliveAck => TryProcessHeartbeat(Account, Character),
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
            var state = (await Service.AccountStateCache.GetAsync<MigrationState>(Account.ID.ToString())).Value;
            if (state != MigrationState.Migrating)
            {
                await Service.AccountStateCache.RemoveAsync(Account.ID.ToString());
            }
        }
    }
}