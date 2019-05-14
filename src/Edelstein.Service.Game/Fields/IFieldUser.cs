using System;
using System.Collections.Generic;
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

        ICollection<IFieldControlledObj> Controlled { get; }
        ICollection<IFieldOwnedObj> Owned { get; }

        Task SendPacket(IPacket packet);
        IPacket GetSetFieldPacket();
    }
}