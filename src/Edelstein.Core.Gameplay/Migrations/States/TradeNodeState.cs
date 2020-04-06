using System.Collections.Generic;
using Edelstein.Core.Distributed.States;

namespace Edelstein.Core.Gameplay.Migrations.States
{
    public class TradeNodeState : DefaultServerNodeState
    {
        public ICollection<byte> Worlds { get; set; }

        public int RegisterFeeMeso { get; set; }
        public int CommissionRate { get; set; }
        public int CommissionBase { get; set; }
        public int AuctionDurationMin { get; set; }
        public int AuctionDurationMax { get; set; }

        public TradeNodeState()
            => Scope = "Trade";
    }
}