using Edelstein.Protocol.Gameplay.Constants;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

internal static partial class UserOnPacketUserConsumeCashItemUseRequestLogs
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Unhandled cash item use {ItemID} ({Type})"
    )]
    internal static partial void LogCashItemUseUnhandled(
        this ILogger logger,
        int itemId,
        CashItemType type
    );
}
