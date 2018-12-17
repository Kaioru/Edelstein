using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Sockets
{
    public class WvsGameSocket : AbstractSocket
    {
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

        public override Task OnPacket(IPacket packet)
        {
            throw new NotImplementedException();
        }

        public override Task OnDisconnect()
        {
            throw new NotImplementedException();
        }

        public override Task OnException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}