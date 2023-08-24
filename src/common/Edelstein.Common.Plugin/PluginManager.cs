using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Plugin;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public class PluginManager<TContext> : Repository<string, IPlugin<TContext>>, IPluginManager<TContext>
{
    private readonly ILogger _logger;
    
    public PluginManager(
        ILogger<PluginManager<TContext>> logger
    ) => _logger = logger;

    public async Task LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            _logger.LogWarning("Failed to load plugins from path {Path} as file does not exist", path);
            return;
        }

        var loader = PluginLoader.CreateFromAssemblyFile(path, config => config.PreferSharedTypes = true);
        var assembly = loader.LoadDefaultAssembly();
        var types = assembly
            .GetTypes()
            .Where(t => typeof(IPlugin<TContext>).IsAssignableFrom(t) && !t.IsAbstract);
        
        foreach (var type in types)
        {
            try
            {
                if (Activator.CreateInstance(type) is not IPlugin<TContext> plugin)
                {
                    _logger.LogWarning("Failed to start plugin of type {Type}", type);
                    continue;
                }

                await Insert(plugin);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load plugin assembly {Assembly}", assembly.Location);
            }
        }
    }
    public async Task LoadFromDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            _logger.LogWarning("Failed to load plugins from path {Path} as directory does not exist", directory);
            return;
        }
        
        foreach (var subdirectory in Directory.GetDirectories(directory))
        {
            var name = Path.GetFileName(subdirectory);
            var file = Path.Combine(subdirectory, name);

            await LoadFromFile($"{file}.dll");
        }
    }
}
