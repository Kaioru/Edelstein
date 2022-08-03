namespace Edelstein.Protocol.Util.Commands;

public interface ICommandContext
{
    Task Message(string message);
}
