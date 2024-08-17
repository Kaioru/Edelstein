using System;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

internal static partial class PluginManagerLogs
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Failed to load plugins from path {Path} as file does not exist"
    )]
    internal static partial void LogPluginManagerFailedFile(
        this ILogger logger,
        string path
    );
    
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Failed to load plugins from path {Path} as directory does not exist"
    )]
    internal static partial void LogPluginManagerFailedDirectory(
        this ILogger logger,
        string path
    );
    
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Failed to create instance plugin of type {Type}"
    )]
    internal static partial void LogPluginManagerFailedCreateInstance(
        this ILogger logger,
        Type type
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to load plugin assembly {Assembly}"
    )]
    internal static partial void LogPluginManagerFailedAssembly(
        this ILogger logger,
        Exception exception,
        string assembly
    );
}
