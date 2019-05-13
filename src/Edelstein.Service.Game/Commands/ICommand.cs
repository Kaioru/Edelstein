using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        ICollection<string> Aliases { get; }
        ICollection<ICommand> Commands { get; }

        Task Process(FieldUser sender, string text);
        Task Process(FieldUser sender, Queue<string> args);
    }
}