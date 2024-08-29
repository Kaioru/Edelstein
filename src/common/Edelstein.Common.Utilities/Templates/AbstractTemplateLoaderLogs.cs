using System;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Utilities.Templates;

internal static partial class AbstractTemplateLoaderLogs
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "{Loader} loaded {Count:n0} {TemplateName} templates in {Elapsed:c}"
    )]
    internal static partial void LogTemplateLoaderLoaded(
        this ILogger logger,
        string loader,
        int count,
        string templateName,
        TimeSpan elapsed
    );
}
