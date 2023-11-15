using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Plugin;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public class PluginManager<TContext> : Repository<string, IPluginHost<TContext>>, IPluginManager<TContext>
{
    private readonly IHostEnvironment _environment;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;

    public PluginManager(IHostEnvironment environment, ILoggerFactory loggerFactory, ILogger<PluginManager<TContext>> logger)
    {
        _environment = environment;
        _loggerFactory = loggerFactory;
        _logger = logger;
    }

    public Task LoadFromFile(string path) => LoadFromFile(path, null);
    
    public async Task LoadFromFile(string path, IPluginHostManifest? manifest)
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

                var directoryHost = AppDomain.CurrentDomain.BaseDirectory;
                var directoryPlugin = Path.GetDirectoryName(path) ?? directoryHost;
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(directoryPlugin)
                    .AddJsonFile("appsettings.json", true)
                    .AddJsonFile($"appsettings.{_environment.EnvironmentName}.json", true)
                    .Build();
                
                await Insert(new PluginHost<TContext>(
                    manifest,
                    _loggerFactory.CreateLogger(type),
                    configuration,
                    directoryHost,
                    directoryPlugin,
                    plugin
                ));
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
            var manifest = new PluginHostManifest();
            
            if (File.Exists(Path.Combine(subdirectory, Path.GetFileName("manifest.json"))))
            {
                var manifestConfiguration = new ConfigurationBuilder()
                    .SetBasePath(subdirectory)
                    .AddJsonFile("manifest.json", true)
                    .Build();
                
                manifestConfiguration.Bind(manifest);

                name = manifest.EntryPoint;
                file = Path.Combine(subdirectory, name);
            } 
            
            await LoadFromFile(Path.ChangeExtension(file, "dll"), manifest);
        }
    }
}
