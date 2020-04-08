namespace Edelstein.Core.Distributed.States
{
    public class DefaultServerNodeState : IServerNodeState
    {
        public string Name { get; set; }
        public string Scope { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public short Version { get; } = 95;
        public string Patch { get; } = "1";
        public byte Locale { get; } = 8;
    }
}