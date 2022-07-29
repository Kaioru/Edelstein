using Edelstein.Protocol.Services.Migration.Types;

namespace Edelstein.Protocol.Services.Migration.Contracts;

public interface IMigrationStartRequest
{
    IMigration Migration { get; }
}
