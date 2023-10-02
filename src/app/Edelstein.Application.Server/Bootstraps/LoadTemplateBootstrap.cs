using System.Collections.Immutable;
using System.Diagnostics;
using Edelstein.Common.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class LoadTemplateBootstrap : IBootstrap
{
    private readonly ICollection<ITemplateLoader> _loaders;
    private readonly ILogger _logger;

    public LoadTemplateBootstrap(
        ILogger<LoadTemplateBootstrap> logger,
        IEnumerable<ITemplateLoader> loaders
    )
    {
        _logger = logger;
        _loaders = loaders.ToImmutableList();
    }

    public int Priority => BootstrapPriority.Load;

    public async Task Start()
    {
        foreach (var loader in _loaders)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var count = await loader.Load();

            _logger.LogInformation(
                "{Loader} loaded {Count} templates in {Elapsed:F3}ms",
                loader.GetType().Name, count, stopwatch.Elapsed.TotalMilliseconds
            );
        }
    }

    public Task Stop() => Task.CompletedTask;
}
