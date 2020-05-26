using System;
using System.Threading.Tasks;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Network
{
    public interface ISocketAdapter
    {
        Task OnPacket(IPacketDecoder packet);
        Task OnException(Exception exception);
        Task OnUpdate();
        Task OnDisconnect();

        Task SendPacket(IPacket packet);
        Task Close();
    }
}