using System;
using System.Threading.Tasks;
using Edelstein.Database;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldUser : IFieldLife, IDisposable
    {
        Character Character { get; }
        bool IsInstantiated { get; set; }

        Task SendPacket(IPacket packet);
        IPacket GetSetFieldPacket();
    }
}