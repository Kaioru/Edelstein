using System.Collections.Generic;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Shop.Templates;

public interface IShopTemplate : ITemplate
{
    ICollection<IShopTemplateItem> Items { get; }
}
