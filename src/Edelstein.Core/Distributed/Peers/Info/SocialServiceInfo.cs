using System.Collections.Generic;

namespace Edelstein.Core.Distributed.Peers.Info
{
    public class SocialServiceInfo : PeerServiceInfo
    {
        public ICollection<byte> Worlds { get; set; }
    }
}