using Edelstein.Protocol.Services.Migration.Types;

namespace Edelstein.Protocol.Services.Migration.Contracts;

public interface IMigrationClaimResponse : IMigrationResponse
{
    IMigration? Migration { get; }
}
