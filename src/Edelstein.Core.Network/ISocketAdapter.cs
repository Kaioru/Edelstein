using System;
using System.Collections.Generic;
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
        Task SendPacket(IEnumerable<IPacket> packets);
        Task Close();
    }
}