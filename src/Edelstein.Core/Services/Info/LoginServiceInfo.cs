using System;
using System.Collections.Generic;

namespace Edelstein.Core.Services.Info
{
    [Serializable]
    public class LoginServiceInfo : ServerServiceInfo
    {
        public ICollection<WorldInfo> Worlds { get; set; }

        [Serializable]
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
    }
}