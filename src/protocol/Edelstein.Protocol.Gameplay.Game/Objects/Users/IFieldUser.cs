using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public interface IFieldUser : IFieldLife, ISocketUser
{
    Account Account { get; }
    AccountWorldData AccountWorldData { get; }
    Character Character { get; }
    
    ICollection<IFieldSplit> Observing { get; }
    
    IDispatchable GetDispatchSetField();
}
