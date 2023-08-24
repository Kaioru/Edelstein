namespace Edelstein.Protocol.Utilities.Repositories;

public interface IIdentifiable<out T>
{
    T ID { get; }
}
