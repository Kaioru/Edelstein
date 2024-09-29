using System;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Templates.Special;
using Edelstein.Protocol.Gameplay.Game.Items.Special;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Items.Special;

public class StatChangeItemUseManager(
    ITemplateManager<IItemTemplate> templates
) : AbstractItemUseManager<IStatChangeItemUse, IItemStatChangeTemplate>(templates)
{
    protected override IStatChangeItemUse Create(IItemStatChangeTemplate template)
        => new StatChangeItemUse(template);

    protected override void Handle(IStatChangeItemUse context, IFieldUserModify modify)
    {
        modify.Stats(s =>
        {
            if (context.HP.HasValue) s.HP += context.HP.Value;
            if (context.MP.HasValue) s.MP += context.MP.Value;
        });
    }
}
