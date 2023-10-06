namespace Edelstein.Plugin.Rue.Commands;

public interface IIndexedCommand : ICommand
{
    Task Index();
}
