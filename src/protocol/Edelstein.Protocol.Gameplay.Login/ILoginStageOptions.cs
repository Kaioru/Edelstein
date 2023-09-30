using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Login;

public interface ILoginStageOptions : IIdentifiable<string>
{
    byte[] Worlds { get; }

    // TODO move this to plugins
    bool IsFlippedUsername { get; }
}
