namespace Edelstein.Protocol.Util.Commands;

public interface ICommandManager<TContext>
    where TContext : ICommandContext
{
    IReadOnlyCollection<ICommand<TContext>> Commands { get; }

    void Register(ICommand<TContext> command);
    void Deregister(ICommand<TContext> command);

    Task<bool> Process(TContext ctx, string text);
    Task<bool> Process(TContext ctx, string[] args);
}
