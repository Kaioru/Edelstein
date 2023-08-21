namespace Edelstein.Protocol.Network.Transports;

public record TransportVersion(
    short Major,
    string Patch,
    byte Locale
);
