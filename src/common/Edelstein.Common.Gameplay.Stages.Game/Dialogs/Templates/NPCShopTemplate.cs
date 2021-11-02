using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Stages.Game.Dialogs;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Stages.Game.Dialogs.Templates
{
    public record NPCShopTemplate : IShopDialogInfo, ITemplate
    {
        public int ID { get; }
        public IDictionary<int, IShopDialogItemInfo> Items { get; }

        public NPCShopTemplate(int id, IDataProperty property)
        {
            ID = id;
            Items = property.Children
                .ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => (IShopDialogItemInfo)new NPCShopItemTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
                );
        }
    }
}
