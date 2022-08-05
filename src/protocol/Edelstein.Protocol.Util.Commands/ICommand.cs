namespace Edelstein.Protocol.Util.Commands;

public interface ICommand<in TContext> : ICommandExecutable<TContext>
    where TContext : ICommandContext
{
    string Name { get; }
    string Description { get; }

    IEnumerable<string> Aliases { get; }

    void AddAlias(string alias);
    void RemoveAlias(string alias);
}
