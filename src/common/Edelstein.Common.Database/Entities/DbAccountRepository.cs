using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities;

public class DbAccountRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : IAccountRepository
{
    public async Task<IAccount?> Retrieve(int key)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Accounts.FindAsync(key);
        return entity != null ? mapper.Map<Account>(entity) : null;
    }

    public async Task<IAccount> Insert(IAccount entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = mapper.Map<DbAccount>(entry);
        db.Accounts.Add(entity);
        await db.SaveChangesAsync();
        return mapper.Map<Account>(entity);
    }

    public async Task<IAccount> Update(IAccount entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = mapper.Map<DbAccount>(entry);
        db.Accounts.Update(entity);
        await db.SaveChangesAsync();
        return mapper.Map<Account>(entity);
    }

    public async Task Delete(int key)
    {
        await using var db = await factory.CreateDbContextAsync();
        await db.Accounts.Where(a => a.ID == key).ExecuteDeleteAsync();
    }

    public async Task Delete(IAccount entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        await db.Accounts.Where(a => a.ID == entry.ID).ExecuteDeleteAsync();
    }

    public async Task<IAccount?> RetrieveByUsername(string username)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Accounts
            .FirstOrDefaultAsync(a => a.Username.Equals(username));
        return entity != null ? mapper.Map<Account>(entity) : null;
    }
}
