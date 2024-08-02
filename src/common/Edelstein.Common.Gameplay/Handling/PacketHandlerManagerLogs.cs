using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Handling;

internal static partial class PacketHandlerManagerLogs
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Set packet handler for operation 0x{Operation:X} ({OperationName}) to {Handler}"
    )]
    internal static partial void LogPacketHandlerAdded(
        this ILogger logger,
        short operation,
        string operationName,
        string handler
    );
    
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Overriding packet handler for operation 0x{Operation:X} ({OperationName}) to {Handler}"
    )]
    internal static partial void LogPacketHandlerOverridden(
        this ILogger logger,
        short operation,
        string operationName,
        string handler
    );
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Handled packet operation 0x{Operation:X} ({OperationName})"
    )]
    internal static partial void LogPacketHandlerHandled(
        this ILogger logger,
        short operation,
        string operationName
    );
    
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Handler not found for packet operation 0x{Operation:X} ({OperationName})"
    )]
    internal static partial void LogPacketHandlerNotFound(
        this ILogger logger,
        short operation,
        string operationName
    );
}
