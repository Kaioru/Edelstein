using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Services;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Login.Sockets
{
    public partial class WvsLoginSocket : AbstractSocket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public WvsLogin WvsLogin { get; }
        public Account Account { get; set; }

        public WvsLoginSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsLogin wvsLogin
        ) : base(channel, seqSend, seqRecv)
        {
            WvsLogin = wvsLogin;
        }

        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            switch (operation)
            {
                case RecvPacketOperations.CheckPassword:
                    return OnCheckPassword(packet);
                case RecvPacketOperations.WorldInfoRequest:
                case RecvPacketOperations.WorldRequest:
                    return OnWorldInfoRequest(packet);
                case RecvPacketOperations.CheckUserLimit:
                    return OnCheckUserLimit(packet);
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    return Task.CompletedTask;
            }
        }

        public override async Task OnDisconnect()
        {
            if (Account != null)
            {
                var state = await WvsLogin.AccountStatusCache.GetAsync<AccountState>(Account.ID.ToString());

                if (state.HasValue && state.Value != AccountState.MigratingIn)
                    await WvsLogin.AccountStatusCache.RemoveAsync(Account.ID.ToString());
            }
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Caught exception in socket handling");
            return Task.CompletedTask;
        }
    }
}