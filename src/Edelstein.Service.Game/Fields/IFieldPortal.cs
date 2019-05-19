using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields
{
    public interface IFieldPortal
    {
        Task Enter(IFieldUser user);
        Task Leave(IFieldUser user);
    }
}