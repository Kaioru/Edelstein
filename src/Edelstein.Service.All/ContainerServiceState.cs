using System.Collections.Generic;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Gameplay.Migrations.States;

namespace Edelstein.Service.All
{
    public class ContainerServiceState
    {
        public ICollection<LoginNodeState> LoginServices { get; set; }
        public ICollection<GameNodeState> GameServices { get; set; }
        public ICollection<ShopNodeState> ShopServices { get; set; }
        public ICollection<TradeNodeState> TradeServices { get; set; }
    }
}