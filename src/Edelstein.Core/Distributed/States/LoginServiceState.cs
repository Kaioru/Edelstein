using System.Collections.Generic;

namespace Edelstein.Core.Distributed.States
{
    public class LoginServiceState : ServerServiceState
    {
        public bool AutoRegister { get; set; } = false;
        public ICollection<LoginServiceWorldState> Worlds { get; set; }

        public class LoginServiceWorldState
        {
            public byte ID { get; set; }
            public string Name { get; set; }
            public byte State { get; set; }
            public string EventDesc { get; set; }
            public short EventEXP { get; set; }
            public short EventDrop { get; set; }
            public bool BlockCharCreation { get; set; }
            public int UserLimit { get; set; } = 1000;
        }
    }
}