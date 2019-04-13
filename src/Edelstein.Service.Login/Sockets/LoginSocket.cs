using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Sockets
{
    public class LoginSocket : AbstractSocket
    {
        public WvsLogin WvsLogin { get; }

        public LoginSocket(
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

        public override Task OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override Task OnDisconnect()
        {
            throw new NotImplementedException();
        }
    }
}