using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Common.Gameplay.Entities.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Stats;
using Edelstein.Protocol.Gameplay.Entities.Stats.Modifiers;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Objects.Users;

public class FieldUserModify(
    IFieldUser user
) : IFieldUserModify
{
    public bool IsRequireUpdate { get; private set; }
    public bool IsRequireUpdateAvatar { get; private set; }

    public Task Stats(Action<IModifyStatContext>? action = null, bool exclRequest = false)
    {
        var context = new ModifyStatContext(user.Character);
        
        action?.Invoke(context);
        
        if (context.Flag > 0)
            IsRequireUpdate = true;
        
        return user.Dispatch(new StatChanged
        {
            ExclRequest = exclRequest,
            Stats = context.GetDispatch()
        });
    }
    
    public async Task TemporaryStats(Action<IModifyTemporaryStatContext> action, short delay = 0)
    {
        var context = new ModifyTemporaryStatContext(user.Character.TemporaryStats);
        
        action.Invoke(context);
        
        var isUpdateReset = context.StatsReset.Records.Any() ||
                            context.StatsReset.HasTwoStateStats();
        var isUpdateSet = context.StatsSet.Records.Any() ||
                          context.StatsSet.HasTwoStateStats();
        
        if (!IsRequireUpdate)
            IsRequireUpdate = isUpdateReset || isUpdateSet;

        if (isUpdateReset)
        {
            var flag = context.StatsReset.GetFlags().ToArray();

            await user.Dispatch(new TemporaryStatReset
            {
                Flag = flag
            });
            if (user.FieldSplit != null)
                await user.FieldSplit.Dispatch(new UserTemporaryStatReset
                {
                    ObjectID = user.ObjectID ?? 0,
                    Flag = flag
                }, user);
        }

        if (isUpdateSet)
        {
            await user.Dispatch(new TemporaryStatSet
            {
                Stats = context.StatsSet.ToStructuredLocal(),
                Delay = delay
            });
            if (user.FieldSplit != null)
                await user.FieldSplit.Dispatch(new UserTemporaryStatSet
                {
                    ObjectID = user.ObjectID ?? 0,
                    Stats = context.StatsSet.ToStructuredRemote(),
                    Delay = delay
                }, user);
        }
    }

    public Task Inventory(Action<IModifyInventoryContextGroup>? action = null, bool exclRequest = false)
    {
        var context = new ModifyInventoryContextGroup(
            user.Character.Inventories,
            user.System.Context.Templates.Items
        );
        
        action?.Invoke(context);
        
        if (context.IsUpdated)
            IsRequireUpdate = true;
        if (context.IsUpdatedAvatar)
            IsRequireUpdateAvatar = true;

        return user.Dispatch(new InventoryOperation
        {
            ExclRequest = exclRequest,
            Operations = context.GetDispatch(),
            SN = (byte)Random.Shared.Next(byte.MinValue, byte.MaxValue)
        });
    }
}
