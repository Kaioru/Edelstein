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
        var watch0 = new Stopwatch();
        
        watch0.Start();
        
        foreach (var loader in _loaders)
        {
            var watch1 = new Stopwatch();

            watch1.Start();

            var count = await loader.Load();

            _logger.LogInformation(
                "{Loader} loaded {Count} templates in {Elapsed:F2}ms",
                loader.GetType().Name, count, watch1.Elapsed.TotalMilliseconds
            );
        }
        
        _logger.LogInformation(
            "Finished executing {Count} template loaders in {Elapsed:F2}s",
            _loaders.Count, watch0.Elapsed.TotalSeconds
        );
    }

    public Task Stop() => Task.CompletedTask;
}
