using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session.Contracts;

public interface IMigrationClaimResponse : IMigrationResponse
{
    IMigration? Migration { get; }
}
