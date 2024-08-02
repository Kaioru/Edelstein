namespace Edelstein.Protocol.Network.Transports;

public record struct TransportVersion(
    short Major,
    string Patch,
    byte Locale
);
