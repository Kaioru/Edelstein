using Edelstein.Protocol.Plugin;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public class PluginManager<TContext> : IPluginManager<TContext>
{
    private readonly TContext _context;
    private readonly ICollection<IPlugin<TContext>> _loaded;
    private readonly ICollection<PluginLoader> _loaders;
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly Type[] _shared;

    public PluginManager(
        ILogger<PluginManager<TContext>> logger,
        ILoggerFactory loggerFactory,
        TContext context
    )
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _context = context;
        _loaders = new List<PluginLoader>();
        _loaded = new List<IPlugin<TContext>>();
        _shared = new[] { typeof(IPlugin<TContext>), typeof(TContext) };
    }

    public Task Load(string path)
    {
        if (!File.Exists(path))
        {
            _logger.LogWarning("Failed to load plugins from path {Path} as file does not exist", path);
            return Task.CompletedTask;
        }

        _loaders.Add(PluginLoader.CreateFromAssemblyFile(path, _shared));
        return Task.CompletedTask;
    }

    public async Task LoadFrom(string directory)
    {
        foreach (var subdirectory in Directory.GetDirectories(directory))
        {
            var name = Path.GetFileName(subdirectory);
            var file = Path.Combine(subdirectory, name);

            await Load($"{file}.dll");
        }
    }

    public async Task Start()
    {
        foreach (var loader in _loaders)
        {
            var assembly = loader.LoadDefaultAssembly();

            try
            {
                var types = assembly
                    .GetTypes()
                    .Where(t => typeof(IPlugin<TContext>).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var type in types)
                {
                    if (Activator.CreateInstance(type) is not IPlugin<TContext> plugin)
                    {
                        _logger.LogWarning("Failed to start plugin of type {Type}", type);
                        continue;
                    }

                    await plugin.OnStart(_loggerFactory.CreateLogger<IPlugin<TContext>>(), _context);
                    _loaded.Add(plugin);
                    _logger.LogInformation("{Type} plugin started", type);
                }
            }
            catch (Exception)
            {
                _logger.LogError("Failed to load plugin assembly {Assembly}", assembly.Location);
            }
        }
    }

    public async Task Stop()
    {
        foreach (var plugin in _loaded)
        {
            await plugin.OnStop(_loggerFactory.CreateLogger<IPlugin<TContext>>());
            _logger.LogInformation("{Type} plugin stopped", plugin.GetType());
        }

        _loaded.Clear();
    }
}
