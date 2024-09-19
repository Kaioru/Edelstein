using System;
using Edelstein.Protocol.Utilities.Repositories;
using Microsoft.Extensions.Logging;

namespace Edelstein.Protocol.Plugin;

public interface IPluginHost<TContext> : IRepositoryEntry<string>
{
    IPluginHostManifest? Manifest { get; }
    IPluginManager<TContext> Manager { get; }
    IPlugin<TContext> Plugin { get; }
    
    ILogger Logger { get; }
    
    string DirectoryApp { get; }
    string DirectoryPlugin { get; }

    void Export<T>();
    void Import<T>();

    T? ExportMethod<T>(string name, T method) where T : Delegate;
    T? ImportMethod<T>(string name) where T : Delegate;
}
