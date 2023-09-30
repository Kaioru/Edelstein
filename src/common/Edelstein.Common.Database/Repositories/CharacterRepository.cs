using System.Collections.Immutable;
using AutoMapper;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Database.Repositories;

public class CharacterRepository : ICharacterRepository
{
    private readonly IDbContextFactory<GameplayDbContext> _dbFactory;
    private readonly IMapper _mapper;

    public CharacterRepository(IDbContextFactory<GameplayDbContext> dbFactory, IMapper mapper)
    {
        _dbFactory = dbFactory;
        _mapper = mapper;
    }

    public async Task<ICharacter?> Retrieve(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.Characters.FindAsync(key);
        return entity != null ? _mapper.Map<Character>(entity) : null;
    }

    public async Task<ICharacter> Insert(ICharacter entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = _mapper.Map<CharacterEntity>(entry);
        db.Characters.Add(entity);
        await db.SaveChangesAsync();
        return _mapper.Map<Character>(entity);
    }

    public async Task<ICharacter> Update(ICharacter entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = _mapper.Map<CharacterEntity>(entry);
        db.Characters.Update(entity);
        await db.SaveChangesAsync();
        return _mapper.Map<Character>(entity);
    }

    public async Task Delete(int key)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Characters.Remove(new CharacterEntity { ID = key });
        await db.SaveChangesAsync();
    }

    public async Task Delete(ICharacter entry)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Characters.Remove(new CharacterEntity { ID = entry.ID });
        await db.SaveChangesAsync();
    }

    public async Task<bool> CheckExistsByName(string name)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Characters.AnyAsync(c => c.Name.ToLower() == name.ToLower());
    }
    
    public async Task<ICharacter?> RetrieveByName(string name)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.Characters.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        return entity != null ? _mapper.Map<Character>(entity) : null;
    }

    public async Task<ICharacter?> RetrieveByAccountWorldAndCharacter(int accountWorld, int character)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var entity = await db.Characters.FirstOrDefaultAsync(c => c.AccountWorldID == accountWorld && c.ID == character);
        return entity != null ? _mapper.Map<Character>(entity) : null;
    }

    public async Task<IEnumerable<ICharacter>> RetrieveAllByAccountWorld(int accountWorld)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var results = await db.Characters.Where(c => c.AccountWorldID == accountWorld).ToListAsync();
        return results
            .Select(m => _mapper.Map<Character>(m))
            .ToImmutableArray();
    }
}
