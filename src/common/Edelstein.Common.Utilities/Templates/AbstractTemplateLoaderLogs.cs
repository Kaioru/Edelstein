using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Utilities.Templates;

internal static partial class AbstractTemplateLoaderLogs
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "{Loader} loaded {Count} templates in {Elapsed:F2}ms"
    )]
    internal static partial void LogTemplateLoaderLoaded(
        this ILogger logger,
        string loader,
        int count,
        double elapsed
    );
}
