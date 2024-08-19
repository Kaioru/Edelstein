using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities;

public class DbAccountRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : DbRepository<int, DbAccount, Account>(factory, mapper, db => db.Accounts), 
    IAccountRepository
{
    public async Task<Account?> RetrieveByUsername(string username)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Accounts
            .FirstOrDefaultAsync(a => a.Username.Equals(username));
        return entity != null ? mapper.Map<Account>(entity) : null;
    }
}
