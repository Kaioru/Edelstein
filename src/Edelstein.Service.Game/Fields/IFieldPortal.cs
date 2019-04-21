using System.Threading.Tasks;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldPortal
    {
        Task Enter(IFieldUser user);
        Task Leave(IFieldUser user);
    }
}