using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Network
{
    public interface IServer
    {
        ICollection<ISocket> Sockets { get; }

        Task Start(string host, int port);
        Task Stop();
    }
}