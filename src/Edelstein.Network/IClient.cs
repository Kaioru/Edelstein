using System.Threading.Tasks;

namespace Edelstein.Network
{
    public interface IClient
    {
        ISocket Socket { get; }

        Task Start(string host, int port);
        Task Stop();
    }
}