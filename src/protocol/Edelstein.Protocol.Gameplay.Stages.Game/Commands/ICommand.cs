using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Commands
{
    public interface ICommand : ICommandProcessor, ICommandExecutable
    {
        string Name { get; }
        string Description { get; }

        ICollection<string> Aliases { get; }
    }
}
