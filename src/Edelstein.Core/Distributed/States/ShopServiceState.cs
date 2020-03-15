using System.Collections.Generic;

namespace Edelstein.Core.Distributed.States
{
    public class ShopServiceState : ServerServiceState
    {
        public ICollection<byte> Worlds { get; set; }
    }
}