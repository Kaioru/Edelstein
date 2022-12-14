namespace Edelstein.Protocol.Util.Storages;

public interface IIdentifiable<out T>
{
    T ID { get; }
}
