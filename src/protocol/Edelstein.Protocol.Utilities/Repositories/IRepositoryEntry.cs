namespace Edelstein.Protocol.Utilities.Repositories;

public interface IRepositoryEntry<out T>
{
    T ID { get; }
}
