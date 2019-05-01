using System.Collections.Generic;

namespace Edelstein.Core.Distributed.Peers.Info
{
    public class ShopServiceInfo : ServerServiceInfo
    {
        public ICollection<byte> Worlds { get; set; }
    }
}