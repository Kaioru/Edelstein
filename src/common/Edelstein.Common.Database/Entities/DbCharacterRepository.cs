using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Entities;

public class DbCharacterRepository(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper
) : DbRepository<int, DbCharacter, Character>(factory, mapper, db => db.Characters), 
    ICharacterRepository
{
    public async Task<bool> CheckExistsByName(string name)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Characters
            .AnyAsync(c => c.Name.ToLower() == name.ToLower());
    }
    
    public async Task<Character?> RetrieveByName(string name)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Characters
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        return entity != null ? mapper.Map<Character>(entity) : null;
    }
    
    public async Task<Character?> RetrieveByAccountWorldDataAndCharacter(int accountWorldData, int character)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entity = await db.Characters
            .Include(c => c.AccountWorldData)
            .Where(c => c.AccountWorldData.ID == accountWorldData)
            .Where(c => c.ID == character)
            .FirstOrDefaultAsync();
        return entity != null ? mapper.Map<Character>(entity) : null;
    }
    
    public async Task<IEnumerable<Character>> RetrieveAllByAccountWorldData(int accountWorldData)
    {
        await using var db = await factory.CreateDbContextAsync();
        var entities = await db.Characters
            .Include(c => c.AccountWorldData)
            .Where(c => c.AccountWorldData.ID == accountWorldData)
            .ToListAsync();
        return entities
            .Select(mapper.Map<Character>)
            .ToList();
    }
}
