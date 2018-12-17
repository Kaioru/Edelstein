using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Services;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Logging;

namespace Edelstein.Service.Login.Sockets
{
    public partial class WvsLoginSocket : AbstractSocket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public WvsLogin WvsLogin { get; }

        public WvsLoginSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsLogin wvsLogin
        ) : base(channel, seqSend, seqRecv)
        {
            WvsLogin = wvsLogin;
        }

        public override async Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            switch (operation)
            {
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    break;
            }
        }

        public override Task OnDisconnect()
        {
            throw new NotImplementedException();
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Caught exception in socket handling");
            return Task.CompletedTask;
        }
    }
}