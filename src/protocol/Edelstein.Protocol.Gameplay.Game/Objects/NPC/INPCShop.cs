using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC;

public interface INPCShop : IIdentifiable<int>
{
    ICollection<INPCShopItem> Items { get; }
}
