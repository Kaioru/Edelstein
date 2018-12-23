using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.Service.Trade.Sockets
{
    public class WvsTradeSocketFactory : ISocketFactory
    {
        private readonly WvsTrade _wvsShop;

        public WvsTradeSocketFactory(WvsTrade wvsTrade)
            => _wvsShop = wvsTrade;

        public ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new WvsTradeSocket(channel, seqSend, seqRecv, _wvsShop);
    }
}