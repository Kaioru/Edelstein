using AutoMapper;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Repositories;

public class AccountWorldRepository : IAccountWorldRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;
    private readonly IMapper _mapper;
    
    public AccountWorldRepository(IDbContextFactory<GameplayDbContext> dbFactory, IMapper mapper)
    {
        _dbFactory = dbFactory;
        _mapper = mapper;
    }

    public async Task<IAccountWorld?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.AccountWorlds.FindAsync(key);
        return entity != null ? _mapper.Map<AccountWorld>(entity) : null;
    }

    public async Task<IAccountWorld> Insert(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = _mapper.Map<AccountWorldEntity>(entry);
        db.AccountWorlds.Add(entity);
        await db.SaveChangesAsync();
        return _mapper.Map<AccountWorld>(entity);
    }

    public async Task<IAccountWorld> Update(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = _mapper.Map<AccountWorldEntity>(entry);
        db.AccountWorlds.Update(entity);
        await db.SaveChangesAsync();
        return _mapper.Map<AccountWorld>(entity);
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.AccountWorlds.Remove(new AccountWorldEntity { ID = key });
        await db.SaveChangesAsync();
    }

    public async Task Delete(IAccountWorld entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.AccountWorlds.Remove(new AccountWorldEntity { ID = entry.ID });
        await db.SaveChangesAsync();
    }

    public async Task<IAccountWorld?> RetrieveByAccountAndWorld(int accountID, int worldID)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.AccountWorlds
            .FirstOrDefaultAsync(a => a.AccountID == accountID && a.WorldID == worldID);
        return entity != null ? _mapper.Map<AccountWorld>(entity) : null;
    }
}
