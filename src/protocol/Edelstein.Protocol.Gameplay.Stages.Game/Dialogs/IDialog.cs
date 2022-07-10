using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Dialogs
{
    public interface IDialog
    {
        Task Enter(IFieldObjUser user);
        Task Leave(IFieldObjUser user);
    }
}
