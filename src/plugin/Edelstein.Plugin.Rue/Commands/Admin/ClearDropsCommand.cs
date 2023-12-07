using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class ClearDropsCommand : AbstractCommand
{
    public override string Name => "ClearDrops";
    public override string Description => "Clears all drops in the map";
    
    public override async Task Execute(IFieldUser user, string[] args)
    {
        if (user.Field == null) return;
        foreach (var drop in user.Field.GetPool(FieldObjectType.Drop)?.Objects ?? ImmutableArray<IFieldObject>.Empty)
            await user.Field.Leave(drop);
    }
}
