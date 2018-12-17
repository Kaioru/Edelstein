using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Sockets
{
    public class WvsLoginSocket : AbstractSocket
    {
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