using System;
using Edelstein.Core.Services.Info;

namespace Edelstein.Core.Services.Peers
{
    [Serializable]
    public class PeerServiceStatusMessage
    {
        public PeerServiceStatus Status { get; set; }
        public ServiceInfo Info { get; set; }
    }
}