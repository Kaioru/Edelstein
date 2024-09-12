using System;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Plugin;

public interface IPluginHost<TContext> : IRepositoryEntry<string>
{
    IPluginHostManifest? Manifest { get; }
    IPluginManager<TContext> Manager { get; }
    IPlugin<TContext> Plugin { get; }

    void Export(Type type);
    void Import(Type type);
}
