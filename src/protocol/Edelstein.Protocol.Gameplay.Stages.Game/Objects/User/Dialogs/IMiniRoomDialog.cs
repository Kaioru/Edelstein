using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Dialogs
{
    public interface IMiniRoomDialog : IDialog
    {
        IEnumerable<IFieldObjUser> Users { get; }
    }
}
