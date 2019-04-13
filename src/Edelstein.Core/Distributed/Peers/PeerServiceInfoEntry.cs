using System;

namespace Edelstein.Core.Distributed.Peers
{
    public class PeerServiceInfoEntry
    {
        public PeerServiceInfo Info { get; set; }
        public DateTime Expiry { get; set; }
    }
}