using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IMigrationStartRequest
{
    IMigration Migration { get; }
}
