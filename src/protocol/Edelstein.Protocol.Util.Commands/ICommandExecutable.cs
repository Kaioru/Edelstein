namespace Edelstein.Protocol.Util.Commands;

public interface ICommandExecutable<in TContext>
    where TContext : ICommandContext
{
    Task Execute(TContext ctx, string[] args);
}
