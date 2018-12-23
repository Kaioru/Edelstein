using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        ICollection<string> Aliases { get; }
        ICollection<ICommand> Commands { get; }

        Task Process(ICommandSender sender, string text);
        Task Process(ICommandSender sender, IEnumerable<string> args);
    }
}