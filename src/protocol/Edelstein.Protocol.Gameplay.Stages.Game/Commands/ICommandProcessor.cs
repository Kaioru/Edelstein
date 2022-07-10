using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Commands
{
    public interface ICommandProcessor
    {
        ICollection<ICommand> Commands { get; }

        void Register(ICommand command);
        void Deregister(ICommand command);

        Task<bool> Process(IFieldObjUser user, string text);
        Task<bool> Process(IFieldObjUser user, string[] args);
    }
}
