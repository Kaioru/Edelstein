using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageOptions : IIdentifiable<string>
{
    byte[] Worlds { get; }
}
