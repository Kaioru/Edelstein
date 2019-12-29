using System.Collections.Generic;

namespace Edelstein.Core.Distributed.States
{
    public class LoginServiceState : IServerNodeState
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public short Version { get; set; }
        public string Patch { get; set; }
        public byte Locale { get; set; }

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
        }
    }
}