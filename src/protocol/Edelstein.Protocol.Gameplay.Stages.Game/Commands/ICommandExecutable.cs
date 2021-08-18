using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Commands
{
    public interface ICommandExecutable
    {
        Task Execute(IFieldObjUser user, string[] args);
    }
}
