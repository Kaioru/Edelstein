using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Dialogs.MiniRoom
{
    public interface IMiniRoomDialog : IDialog
    {
        IEnumerable<IFieldObjUser> Users { get; }
    }
}
