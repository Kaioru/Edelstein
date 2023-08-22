using AutoMapper;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;
    private readonly IMapper _mapper;

    public AccountRepository(IDbContextFactory<GameplayDbContext> dbFactory, IMapper mapper)
    {
        _dbFactory = dbFactory;
        _mapper = mapper;
    }


    public async Task<IAccount?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.Accounts.FindAsync(key);
        return entity != null ? _mapper.Map<Account>(entity) : null;
    }

    public async Task<IAccount> Insert(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = _mapper.Map<AccountEntity>(entry);
        db.Accounts.Add(entity);
        await db.SaveChangesAsync();
        return _mapper.Map<Account>(entity);
    }

    public async Task<IAccount> Update(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = _mapper.Map<AccountEntity>(entry);
        db.Accounts.Update(entity);
        await db.SaveChangesAsync();
        return _mapper.Map<Account>(entity);
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Accounts.Remove(new AccountEntity { ID = key });
        await db.SaveChangesAsync();
    }

    public async Task Delete(IAccount entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Accounts.Remove(new AccountEntity { ID = entry.ID });
        await db.SaveChangesAsync();
    }

    public async Task<IAccount?> RetrieveByUsername(string username)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.Accounts
            .FirstOrDefaultAsync(a => a.Username.Equals(username));
        return entity != null ?_mapper.Map<Account>(entity) : null;
    }
}
