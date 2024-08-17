using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server;

internal static partial class SystemHostLogs
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{ID} socket acceptor for v{Version}.{Patch} (Locale {Locale}) bound at {Host}:{Port}"
    )]
    internal static partial void LogSystemHostStarted(
        this ILogger logger,
        string id,
        int version,
        string patch,
        byte locale,
        string host,
        int port
    );
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{ID} socket acceptor shutting down, this may take awhile.."
    )]
    internal static partial void LogSystemHostStopping(
        this ILogger logger,
        string id
    );
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{ID} socket acceptor finished shutting down"
    )]
    internal static partial void LogSystemHostStopped(
        this ILogger logger,
        string id
    );
}
