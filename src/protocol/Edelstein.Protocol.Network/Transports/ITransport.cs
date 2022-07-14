namespace Edelstein.Protocol.Network.Transports;

public interface ITransport
{
    ISocketAdapterInitializer Initializer { get; }
    
    short Version { get; }
    string Patch { get; }
    byte Locale { get; }
}
