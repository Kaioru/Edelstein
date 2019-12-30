using System.Collections.Generic;

namespace Edelstein.Core.Distributed.States
{
    public class GameServiceState : ServerServiceState
    {
        public byte ChannelID { get; set; }
        public ICollection<byte> Worlds { get; set; }
    }
}