using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.Service.Login
{
    public class WvsLoginSocketFactory : ISocketFactory
    {
        private readonly WvsLogin _wvsLogin;

        public WvsLoginSocketFactory(WvsLogin wvsLogin)
        {
            _wvsLogin = wvsLogin;
        }

        public ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new WvsLoginSocket(channel, seqSend, seqRecv, _wvsLogin);
    }
}