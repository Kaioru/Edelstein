using Edelstein.Protocol.Services.Server.Contracts;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Services;

internal static partial class SystemServerRegistryHostServiceLogs
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Registered stage {ID} in server registry"
    )]
    internal static partial void LogSystemServerRegistryHostRegistered(
        this ILogger logger,
        string id
    );
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Updated stage {ID} in server registry"
    )]
    internal static partial void LogSystemServerRegistryHostUpdated(
        this ILogger logger,
        string id
    );
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Failed to register stage {ID} in server registry due to {Reason}"
    )]
    internal static partial void LogSystemServerRegistryHostRegisterFailed(
        this ILogger logger,
        string id,
        ServerServiceResult reason
    );
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Failed to update stage {ID} in server registry due to {Reason}"
    )]
    internal static partial void LogSystemServerRegistryHostUpdateFailed(
        this ILogger logger,
        string id,
        ServerServiceResult reason
    );
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Deregistered stage {ID} from server registry"
    )]
    internal static partial void LogSystemServerRegistryHostDeregistered(
        this ILogger logger,
        string id
    );
}
