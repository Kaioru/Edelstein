using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.Service.Game.Sockets
{
    public class WvsGameSocketFactory : ISocketFactory
    {
        private readonly WvsGame _wvsGame;

        public WvsGameSocketFactory(WvsGame wvsGame)
            => _wvsGame = wvsGame;

        public ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new WvsGameSocket(channel, seqSend, seqRecv, _wvsGame);
    }
}