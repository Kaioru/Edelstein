using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Plugin.Rue.Commands;

public interface ICommand : 
    ICommandManager, 
    ICommandExecutable, 
    IIdentifiable<string>
{
    string Name { get; }
    string Description { get; }
    
    ICollection<string> Aliases { get; }
}
