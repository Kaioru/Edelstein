namespace Edelstein.Protocol.Util.Commands;

public interface ICommand<TContext> : ICommandManager<TContext>, ICommandExecutable<TContext>
{
    string Name { get; }
    string Description { get; }

    IEnumerable<string> Aliases { get; }

    void AddAlias(string alias);
    void RemoveAlias(string alias);
}
