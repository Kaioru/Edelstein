using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Dialogs
{
    public interface IDialog
    {
        Task Enter(IFieldObjUser user);
        Task Leave(IFieldObjUser user);
    }
}
