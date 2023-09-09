using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Repositories.Methods;

namespace Edelstein.Plugin.Rue.Commands;

public interface ICommandManager : 
    IRepositoryMethodRetrieve<string, ICommand>,
    IRepositoryMethodRetrieveAll<string, ICommand>,
    IRepositoryMethodInsert<string, ICommand>,
    IRepositoryMethodDelete<string, ICommand>
{
    Task<bool> Process(IFieldUser user, string text);
    Task<bool> Process(IFieldUser user, string[] args);
}
