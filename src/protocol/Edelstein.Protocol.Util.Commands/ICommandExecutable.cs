namespace Edelstein.Protocol.Util.Commands;

public interface ICommandExecutable<in TContext>
{
    Task Execute(TContext ctx, string[] args);
}
