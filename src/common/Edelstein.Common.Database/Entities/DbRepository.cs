using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Edelstein.Protocol.Utilities.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities;

public class DbRepository<TKey, TEntityDb, TEntityDto>(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper,
    Func<GameDbContext, DbSet<TEntityDb>> selector
) : IQueriedRepository<TKey, TEntityDto> 
    where TKey : IEquatable<TKey>
    where TEntityDb : class, IRepositoryEntry<TKey>
    where TEntityDto : class, IRepositoryEntry<TKey>
{
    public async Task<TEntityDto?> Retrieve(TKey key)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await selector.Invoke(db).FindAsync(key);
        return entity != null ? mapper.Map<TEntityDto>(entity) : default;
    }

    public async Task<TEntityDto> Insert(TEntityDto entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = mapper.Map<TEntityDb>(entry);
        selector.Invoke(db).Add(entity);
        await db.SaveChangesAsync();
        return mapper.Map<TEntityDto>(entity);
    }

    public async Task<TEntityDto> Update(TEntityDto entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = mapper.Map<TEntityDb>(entry);
        selector.Invoke(db).Update(entity);
        await db.SaveChangesAsync();
        return mapper.Map<TEntityDto>(entity);
    }

    public async Task Delete(TKey key)
    {
        await using var db = await factory.CreateDbContextAsync();
        await selector.Invoke(db).Where(a => a.ID.Equals(key)).ExecuteDeleteAsync();
    }
    public async Task Delete(TEntityDto entry)
    {
        await using var db = await factory.CreateDbContextAsync();
        await selector.Invoke(db).Where(a => a.ID.Equals(entry.ID)).ExecuteDeleteAsync();
    }
}
