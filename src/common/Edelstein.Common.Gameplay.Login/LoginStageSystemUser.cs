using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageSystemUser(
    ISocket socket, 
    ILoginStageSystem system
) : ILoginStageSystemUser
{
    public ISocket Socket { get; } = socket;
    public ILoginStageSystem System { get; } = system;
    
    public Account? Account { get; set; }
    public AccountWorldData? AccountWorldData { get; set; }
    public Character? Character { get; set; }
    
    public bool IsMigrating { get; set; }
    public long Key { get; set; }
    
    public LoginState State { get; set; }
    
    public byte? SelectedWorldID { get; set; }
    public byte? SelectedChannelID { get; set; }
}
