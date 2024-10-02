using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs;

public interface IDialog
{
    Task OnOpen(IFieldUser user);
    Task OnClose(IFieldUser user);

    Task Close(IFieldUser user);
}
