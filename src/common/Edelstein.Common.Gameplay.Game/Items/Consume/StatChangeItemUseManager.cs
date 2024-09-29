using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public class StatChangeItemUseManager(
    ITemplateManager<IItemTemplate> templates
) : AbstractItemUseManager<IStatChangeItemUseManagerContext, UserStatChangeItemUseRequest, IItemStatChangeTemplate>(templates)
{
    protected override IStatChangeItemUseManagerContext Create(IItemStatChangeTemplate template)
        => new StatChangeItemUseManagerContext(template);

    protected override void Handle(IStatChangeItemUseManagerContext context, IFieldUser user)
    {
        user.ModifyStats(s =>
        {
            if (context.HP.HasValue) s.HP += context.HP.Value;
            if (context.MP.HasValue) s.MP += context.MP.Value;
            if (context.HPr.HasValue) s.HP += (int)(user.Stats.MaxHP * (context.HPr.Value / 100d));
            if (context.MPr.HasValue) s.MP += (int)(user.Stats.MaxMP * (context.MPr.Value / 100d));
        });
    }
}
