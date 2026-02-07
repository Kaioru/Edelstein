using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class DiagnosticLogger(ILogger inner, bool diagnosticsEnabled) : ILogger
{
    private readonly ILogger _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    private readonly bool _diagnosticsEnabled = diagnosticsEnabled;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => _inner.BeginScope(state) ?? NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel)
        => _diagnosticsEnabled
            ? _inner.IsEnabled(logLevel)
            : logLevel >= LogLevel.Warning && _inner.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!_diagnosticsEnabled && logLevel < LogLevel.Warning)
            return;

        if (!_inner.IsEnabled(logLevel))
            return;

        _inner.Log(logLevel, eventId, state, exception, formatter);
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
        }
    }
}
