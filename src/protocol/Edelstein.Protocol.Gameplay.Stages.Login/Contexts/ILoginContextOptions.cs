using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextOptions : IIdentifiable<string>
{
    byte[] Worlds { get; }
}
