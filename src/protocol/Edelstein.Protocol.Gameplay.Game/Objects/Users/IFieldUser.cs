using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public interface IFieldUser : 
    IFieldLife<IFieldUserMovePath, IFieldUserMoveAction>, 
    ISocketUser
{
    IGameStageSystem System { get; }
    
    Account Account { get; }
    AccountWorldData AccountWorldData { get; }
    Character Character { get; }
    
    ICollection<IFieldSplit> Observing { get; }
    
    IFieldUserStats Stats { get; }
    
    bool IsFirstEnter { get; set; }

    IDispatchable GetDispatchSetField();

    Task Initialize();
    Task Modify(Action<IFieldUserModify> action);
}
