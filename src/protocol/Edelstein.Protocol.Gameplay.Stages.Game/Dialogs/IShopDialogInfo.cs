using System.Collections.Generic;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Dialogs
{
    public interface IShopDialogInfo : IRepositoryEntry<int>
    {
        ICollection<IShopDialogInfoItem> Items { get; }
    }
}
