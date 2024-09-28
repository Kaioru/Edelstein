using System.Threading.Tasks;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game;

namespace Edelstein.Common.Gameplay.Game;

public class FieldSetManager(
    IFieldManager fields
) : Repository<string, IFieldSet>, IFieldSetManager
{
    public override async Task<IFieldSet> Insert(IFieldSet entry)
    {
        await entry.Initialize(fields);
        return await base.Insert(entry);
    }

    public override async Task Delete(IFieldSet entry)
    {
        entry.Dispose();
        await base.Delete(entry);
    }

    public override async Task Delete(string key)
    {
        (await Retrieve(key))?.Dispose();
        await base.Delete(key);
    }
}
