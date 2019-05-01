using System;
using System.Threading.Tasks;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Services;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldUser : IFieldLife, IDisposable
    {
        GameService Service { get; }
        GameSocket Socket { get; }
        Character Character { get; }
        bool IsInstantiated { get; set; }

        Task SendPacket(IPacket packet);
        IPacket GetSetFieldPacket();
    }
}