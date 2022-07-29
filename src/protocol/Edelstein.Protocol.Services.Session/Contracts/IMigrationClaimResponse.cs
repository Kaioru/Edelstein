using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IMigrationClaimResponse
{
    MigrationClaimResult Result { get; }
    IMigration? Migration { get; }
}
