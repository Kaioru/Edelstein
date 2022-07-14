namespace Edelstein.Protocol.Util.Repositories;

public interface IIdentifiable<out T>
{
    T ID { get; }
}
