using Edelstein.Protocol.Services.Server;

namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageSystemOptions : IServerInfoLogin
{
    byte[] Worlds { get; }
}
