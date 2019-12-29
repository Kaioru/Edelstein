using System.Collections.Generic;

namespace Edelstein.Core.Distributed.States
{
    public class GameServiceState : IServerNodeState
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public short Version { get; set; }
        public string Patch { get; set; }
        public byte Locale { get; set; }

        public byte ChannelID { get; set; }
        public ICollection<byte> Worlds { get; set; }
    }
}