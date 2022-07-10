using System.Threading.Tasks;
using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Network.Transport
{
    public interface ITransportConnector : ITransport
    {
        ISession Session { get; set; }

        Task Connect(string host, int port);
        Task Close();
    }
}
