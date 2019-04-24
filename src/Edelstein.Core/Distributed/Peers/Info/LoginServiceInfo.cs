using System.Collections.Generic;
using System.Drawing;

namespace Edelstein.Core.Distributed.Peers.Info
{
    public class LoginServiceInfo : ServerServiceInfo
    {
        public bool AutoRegister { get; set; } = true;
        
        public ICollection<WorldInfo> Worlds { get; set; }
        public ICollection<LoginBalloonInfo> Balloons { get; set; }

        public class WorldInfo
        {
            public byte ID { get; set; }
            public string Name { get; set; }
            public byte State { get; set; }
            public string EventDesc { get; set; }
            public short EventEXP { get; set; }
            public short EventDrop { get; set; }
            public bool BlockCharCreation { get; set; }
        }

        public class LoginBalloonInfo
        {
            public Point Position { get; set; }
            public string Message { get; set; }
        }
    }
}