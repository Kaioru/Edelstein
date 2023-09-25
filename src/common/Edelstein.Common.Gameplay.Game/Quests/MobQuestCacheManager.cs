using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Gameplay.Game.Quests;

public class MobQuestCacheManager : IMobQuestCacheManager
{
    private readonly IRepository<int, IMobQuestCache> _cache;
    
    public MobQuestCacheManager() => _cache = new Repository<int, IMobQuestCache>();
    
    public async Task<IMobQuestCache?> Retrieve(int key)
    {
        var record = await _cache.Retrieve(key);

        if (record == null)
        {
            record = new MobQuestCache(key);
            await _cache.Insert(record);
        }

        return record;
    }
}
