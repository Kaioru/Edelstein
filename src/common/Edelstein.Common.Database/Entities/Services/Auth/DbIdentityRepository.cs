using System.Threading.Tasks;
using Edelstein.Common.Services.Auth;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities.Services.Auth;

public class DbIdentityRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : DbRepository<DbIdentity, Identity>(factory, mapper, db => db.Identities), 
    IIdentityRepository
{
    public async Task<Identity?> RetrieveByUsername(string username)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Identities
            .FirstOrDefaultAsync(a => a.Username.Equals(username));
        return entity != null ? mapper.Map<Identity>(entity) : null;
    }
}
