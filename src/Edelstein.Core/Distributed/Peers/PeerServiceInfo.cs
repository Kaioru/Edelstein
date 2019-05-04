using Edelstein.Core.Distributed.Peers.Info;

namespace Edelstein.Core.Distributed.Peers
{
    [MessagePack.Union(0, typeof(LoginServiceInfo))]
    [MessagePack.Union(1, typeof(GameServiceInfo))]
    [MessagePack.Union(2, typeof(ShopServiceInfo))]
    [MessagePack.Union(3, typeof(TradeServiceInfo))]
    public abstract class PeerServiceInfo
    {
        public byte ID { get; set; }
        public string Name { get; set; }
    }
}