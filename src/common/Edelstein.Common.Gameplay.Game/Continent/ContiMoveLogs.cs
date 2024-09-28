using System;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Continent;

internal static partial class ContiMoveLogs
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "{Name} contimove is scheduled to board at {NextBoarding}"
    )]
    internal static partial void LogContiMoveScheduled(
        this ILogger logger,
        string name,
        DateTime nextBoarding
    );
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "{Name} contimove event is scheduled at {NextEvent} to {NextEventEnd}"
    )]
    internal static partial void LogContiMoveScheduledEvent(
        this ILogger logger,
        string name,
        DateTime? nextEvent,
        DateTime? nextEventEnd
    );
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "{Name} contimove started the event, ending at {NextEventEnd}"
    )]
    internal static partial void LogContiMoveEventStarted(
        this ILogger logger,
        string name,
        DateTime? nextEventEnd
    );
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "{Name} contimove ended the event"
    )]
    internal static partial void LogContiMoveEventEnded(
        this ILogger logger,
        string name
    );
    
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "{Name} contimove state triggered {Trigger} and transitioned to {State}, next state change at {NextState}"
    )]
    internal static partial void LogContiMoveStateTrigger(
        this ILogger logger,
        string name,
        ContiMoveStateTrigger trigger,
        ContiMoveState state,
        DateTime? nextState
    );
}
