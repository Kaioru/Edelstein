using System.Collections.Generic;
using Edelstein.Core.Distributed.States;

namespace Edelstein.Service.All
{
    public class ContainerServiceState
    {
        public ICollection<LoginServiceState> LoginServices { get; set; }
        public ICollection<GameServiceState> GameServices { get; set; }
        public ICollection<ShopServiceState> ShopServices { get; set; }
    }
}