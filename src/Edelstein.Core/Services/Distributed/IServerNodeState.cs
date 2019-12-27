namespace Edelstein.Core.Services.Distributed
{
    public interface IServerNodeState : INodeState
    {
        string Type { get; }

        string Host { get; }
        int Port { get; }
    }
}