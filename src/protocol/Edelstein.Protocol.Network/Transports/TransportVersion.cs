namespace Edelstein.Protocol.Network.Transports;

public record TransportVersion(
    short Version,
    string Patch,
    byte Locale
);
