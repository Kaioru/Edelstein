using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Interactions
{
    public interface IDialog
    {
        FieldUser User { get; }
        
        Task Enter();
        Task Leave();
    }
}