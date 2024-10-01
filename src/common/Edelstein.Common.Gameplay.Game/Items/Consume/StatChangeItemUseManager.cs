using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Constants;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Consume;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Items.Consume;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Consume;

public class StatChangeItemUseManager(
    ITemplateManager<IItemTemplate> templates
) : AbstractItemUseManager<IStatChangeItemUseManagerContext, UserStatChangeItemUseRequest, IItemStatChangeTemplate>(templates),
    IStatChangeItemUseManager
{
    protected override IStatChangeItemUseManagerContext Create(
        IFieldUser user,
        ItemSlotBase item,
        IItemStatChangeTemplate template,
        UserStatChangeItemUseRequest info
    )
        => new StatChangeItemUseManagerContext(user, item, template, info);

    protected override async Task<bool> Check(IStatChangeItemUseManagerContext context, IFieldUser user)
    {
        if (!context.Template.ID.IsStatChangeItem())
            return false;
        return user.Field != null &&
               !user.Field.Template.Limit.HasFlag(FieldLimitType.StatChangeItemConsumeLimit);
    }

    protected override async Task Handle(IStatChangeItemUseManagerContext context, IFieldUser user)
    {
        await user.ModifyStats(s =>
        {
            if (context.HP.HasValue) s.HP += context.HP.Value;
            if (context.MP.HasValue) s.MP += context.MP.Value;
            if (context.HPr.HasValue) s.HP += (int)(user.Stats.MaxHP * (context.HPr.Value / 100d));
            if (context.MPr.HasValue) s.MP += (int)(user.Stats.MaxMP * (context.MPr.Value / 100d));
        });
    }
}
