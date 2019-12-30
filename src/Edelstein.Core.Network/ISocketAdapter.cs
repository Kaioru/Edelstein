using System;
using System.Threading.Tasks;
using Edelstein.Network.Packets;

namespace Edelstein.Network
{
    public interface ISocketAdapter
    {
        ISocket Socket { get; }

        Task OnPacket(IPacket packet);
        Task OnException(Exception exception);
        Task OnUpdate();
        Task OnDisconnect();
        
        Task SendPacket(IPacket packet);
        Task Close();
    }
}