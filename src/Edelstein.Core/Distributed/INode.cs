using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Distributed
{
    public interface INode
    {
        Task<IEnumerable<INodeRemote>> GetPeers();

        Task SendMessage<T>(T message) where T : class;
        Task SendMessages<T>(IEnumerable<T> messages) where T : class;

        Task BroadcastMessage<T>(T message) where T : class;
        Task BroadcastMessages<T>(IEnumerable<T> messages) where T : class;
    }
}