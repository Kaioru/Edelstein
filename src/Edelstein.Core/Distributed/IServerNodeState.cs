namespace Edelstein.Core.Distributed
{
    public interface IServerNodeState : INodeState
    {
        string Host { get; }
        int Port { get; }

        short Version { get; }
        string Patch { get; }
        byte Locale { get; }
    }
}