using System.Collections.Generic;
using Edelstein.Core.Distributed.Peers.Info;

namespace Edelstein.Service.All
{
    public class ContainerServiceInfo
    {
        public ICollection<LoginServiceInfo> LoginServices { get; set; }
        public ICollection<GameServiceInfo> GameServices { get; set; }
    }
}