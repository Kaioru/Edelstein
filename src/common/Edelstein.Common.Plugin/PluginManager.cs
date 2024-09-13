using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Plugin;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Plugin;

public class PluginManager<TContext>(
    ILogger<PluginManager<TContext>> logger
) : Repository<string, PluginManagerEntry<TContext>>, IPluginManager<TContext>
{
    public new async Task<IPluginHost<TContext>?> Retrieve(string key) 
        => (await base.Retrieve(key))?.Host;
    
    public new async Task<ICollection<IPluginHost<TContext>>> RetrieveAll() 
        => (await base.RetrieveAll()).Select(h => h.Host).ToList();
    
    public async Task<IPluginHost<TContext>> Insert(IPluginHost<TContext> entry) 
        => (await base.Insert(new PluginManagerEntry<TContext>(entry, entry.Plugin))).Host;

    public Task LoadFromFile(string path) => LoadFromFile(path, null);

    private async Task LoadFromFile(string path, IPluginHostManifest? manifest)
    {
        if (!File.Exists(path))
        {
            logger.LogPluginManagerFailedFile(path);
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
                    logger.LogPluginManagerFailedCreateInstance(type);
                    continue;
                }
                
                await Insert(new PluginManagerEntry<TContext>(
                    new PluginHost<TContext>(manifest, this, plugin),
                    plugin
                ));
            }
            catch (Exception e)
            {
                logger.LogPluginManagerFailedAssembly(e, assembly.Location);
            }
        }
    }
    
    public async Task LoadFromDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            logger.LogPluginManagerFailedDirectory(directory);
            return;
        }
        
        foreach (var subdirectory in Directory.GetDirectories(Path.GetFullPath(directory)))
        {
            var name = Path.GetFileName(subdirectory);
            var file = Path.Combine(subdirectory, name);
            PluginHostManifest? manifest = null;

            if (File.Exists(Path.Combine(subdirectory, Path.GetFileName("manifest.json"))))
            {
                var manifestConfiguration = new ConfigurationBuilder()
                    .SetBasePath(subdirectory)
                    .AddJsonFile("manifest.json", true)
                    .Build();
                
                manifest = manifestConfiguration.Get<PluginHostManifest>();

                if (manifest != null)
                {
                    name = manifest.EntryPoint;
                    file = Path.Combine(subdirectory, name);
                }
            }

            await LoadFromFile(Path.ChangeExtension(file, "dll"), manifest);
        }
    }
    
    public async Task InvokeInit(TContext context)
        => await Task.WhenAll((await base.RetrieveAll()).Select(p => p.Plugin.OnInit(p.Host, context)));
    
    public async Task InvokeStart(TContext context)
        => await Task.WhenAll((await base.RetrieveAll()).Select(p => p.Plugin.OnStart(p.Host, context)));

    public async Task InvokeStop()
        => await Task.WhenAll((await base.RetrieveAll()).Select(p => p.Plugin.OnStop()));
}
