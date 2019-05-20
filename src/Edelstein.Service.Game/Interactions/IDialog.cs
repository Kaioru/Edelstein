using System.Threading.Tasks;

namespace Edelstein.Service.Game.Interactions
{
    public interface IDialog
    {
        Task Enter();
        Task Leave();
    }
}