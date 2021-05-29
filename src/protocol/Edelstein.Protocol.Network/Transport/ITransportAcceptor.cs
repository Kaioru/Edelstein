using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Network.Transport
{
    public interface ITransportAcceptor : ITransport
    {
        IDictionary<string, ISession> Sessions { get; init; }

        Task Accept(string host, int port);
        Task Close();
    }
}
