using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.Service.Shop.Sockets
{
    public class WvsShopSocketFactory : ISocketFactory
    {
        private readonly WvsShop _wvsShop;

        public WvsShopSocketFactory(WvsShop wvsShop)
            => _wvsShop = wvsShop;

        public ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new WvsShopSocket(channel, seqSend, seqRecv, _wvsShop);
    }
}