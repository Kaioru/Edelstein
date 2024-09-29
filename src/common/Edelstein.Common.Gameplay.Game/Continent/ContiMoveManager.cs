using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Continents;

namespace Edelstein.Common.Gameplay.Game.Continent;

public class ContiMoveManager(
    IFieldManager fields
) : Repository<string, IContiMove>, IContiMoveManager
{
    public override async Task<IContiMove> Insert(IContiMove entry)
    {
        await entry.Initialize(fields);
        return await base.Insert(entry);
    }

    public override async Task Delete(IContiMove entry)
    {
        entry.Dispose();
        await base.Delete(entry);
    }

    public override async Task Delete(string key)
    {
        (await Retrieve(key))?.Dispose();
        await base.Delete(key);
    }
    
    public async Task<IContiMove?> RetrieveByField(IField field)
        => (await RetrieveAll())
            .FirstOrDefault(c =>
                c.StartShipMoveField == field ||
                c.WaitField == field ||
                c.MoveField == field ||
                c.CabinField == field
            );
}
