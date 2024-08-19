using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay;

public interface IStageSystemUser<out TStageSystem, TStageSystemUser> : 
    ISocketUser
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    TStageSystem System { get; }
    
    Account? Account { get; set; }
    AccountWorldData? AccountWorldData { get; set; }
    
    long Key { get; set; }
}
