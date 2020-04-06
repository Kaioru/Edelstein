using System.Collections.Generic;
using Edelstein.Core.Distributed.States;

namespace Edelstein.Core.Gameplay.Migrations.States
{
    public class ShopNodeState : DefaultServerNodeState
    {
        public ICollection<byte> Worlds { get; set; }
        
        public ShopNodeState()
            => Scope = "Shop";
    }
}