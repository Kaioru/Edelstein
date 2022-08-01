using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Services.Migration.Contracts;

public interface IMigrationClaimRequest : IIdentifiable<int>
{
    string ServerID { get; }
    long Key { get; }
}
