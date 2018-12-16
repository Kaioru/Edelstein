using System;

namespace Edelstein.Core.Services.Peers
{
    [Serializable]
    public class PeerServiceInfo
    {
        public ServiceInfo Info { get; set; }
        public DateTime Expiry { get; set; }
    }
}