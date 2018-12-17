using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Services;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Sockets
{
    public partial class WvsGameSocket : AbstractSocket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public WvsGame WvsGame { get; }

        public WvsGameSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsGame wvsGame
        ) : base(channel, seqSend, seqRecv)
        {
            WvsGame = wvsGame;
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