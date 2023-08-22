using System.Collections.Immutable;
using System.Diagnostics;
using Edelstein.Common.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class LoadTemplateBootstrap : IBootstrap
{
    private readonly ILogger _logger;
    private readonly ICollection<ITemplateLoader> _loaders;

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
        var stopwatch = new Stopwatch();

        foreach (var loader in _loaders)
        {
            stopwatch.Start();

            var count = await loader.Load();

            _logger.LogInformation(
                "{Loader} loaded {Count} templates in {Elapsed}",
                loader.GetType().Name, count, stopwatch.Elapsed
            );
        }
    }
    
    public Task Stop() => Task.CompletedTask;
}
