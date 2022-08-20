using Edelstein.Protocol.Plugin;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public class PluginManager<TContext> : IPluginManager<TContext>, IPluginCollection<TContext>
{
    private readonly TContext _context;
    private readonly IDictionary<string, IPlugin<TContext>> _loaded;
    private readonly ICollection<PluginLoader> _loaders;
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;

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
        _loaded = new Dictionary<string, IPlugin<TContext>>();
    }

    public Task<IPlugin<TContext>?> Retrieve(string key) =>
        Task.FromResult(_loaded.TryGetValue(key, out var plugin)
            ? plugin
            : null);

    public Task Load(string path)
    {
        if (!File.Exists(path))
        {
            _logger.LogWarning("Failed to load plugins from path {Path} as file does not exist", path);
            return Task.CompletedTask;
        }

        _loaders.Add(PluginLoader.CreateFromAssemblyFile(path, config => config.PreferSharedTypes = true));
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

                    _loaded.Add(plugin.ID, plugin);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load plugin assembly {Assembly}", assembly.Location);
            }
        }

        foreach (var plugin in _loaded.Values)
        {
            await plugin.OnStart(
                new PluginHost<TContext>(_loggerFactory.CreateLogger(plugin.GetType()), this),
                _context
            );
            _logger.LogInformation("Started plugin {ID}", plugin.ID);
        }
    }

    public async Task Stop()
    {
        foreach (var plugin in _loaded.Values)
        {
            await plugin.OnStop();
            _logger.LogInformation("Stopped plugin {ID}", plugin.ID);
        }

        _loaded.Clear();
    }
}
