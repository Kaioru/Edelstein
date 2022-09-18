using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Daemon.Server.Bootstraps;
using Edelstein.Daemon.Server.Configs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server;

public class ProgramHost : IHostedService
{
    private readonly ICollection<IBootstrap> _bootstrap;
    private readonly ProgramConfig _config;
    private readonly ILogger _logger;

    public ProgramHost(ILogger<ProgramHost> logger, ProgramConfig config, IEnumerable<IBootstrap> bootstrap)
    {
        _logger = logger;
        _config = config;
        _bootstrap = bootstrap.ToImmutableList();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var bootstrap in _bootstrap)
            await bootstrap.Start();
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.WhenAll(_bootstrap.AsParallel().Select(b => b.Stop()));
}
