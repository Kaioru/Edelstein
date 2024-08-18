using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Account = Edelstein.Protocol.Gameplay.Entities.Account;

namespace Edelstein.Common.Database.Entities;

public class DbAccountRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : IAccountRepository
{
    public async Task<Account?> Retrieve(int key)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Accounts.FindAsync(key);
        return entity != null ? mapper.Map<Account>(entity) : null;
    }

    public async Task<Account> Insert(Account entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = mapper.Map<DbAccount>(entry);
        db.Accounts.Add(entity);
        await db.SaveChangesAsync();
        return mapper.Map<Account>(entity);
    }

    public async Task<Account> Update(Account entry)
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

    public async Task Delete(Account entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        await db.Accounts.Where(a => a.ID == entry.ID).ExecuteDeleteAsync();
    }

    public async Task<Account?> RetrieveByUsername(string username)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Accounts
            .FirstOrDefaultAsync(a => a.Username.Equals(username));
        return entity != null ? mapper.Map<Account>(entity) : null;
    }
}
